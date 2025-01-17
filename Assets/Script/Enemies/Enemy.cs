using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Shapes { MayorQue, MenorQue, N, Z, I, U, UInvertida }
public abstract class Enemy : MonoBehaviour {

    [Header("UI Content")]
    public GameObject healthbarPrefab;
    [Range(0, 2)] public float healthbarOffsetY;
    [Space]
    public GameObject damagebarPrefab;
    [Range(0, 2)] public float damagebarOffsetY;
    private Slider healthbar, damagebar;
    private Transform uiPosition;

    [Header("Characteristics")]
    [Tooltip("Velocidad de Movimiento")] public float speed;
    [Tooltip("Daño que hace al jugador")] public float damage;
    [Tooltip("Cooldown para lanzar ataque cuando está en rango")] public float delayToTakeDamage = 0.5f;
    [Tooltip("Distancia en la que detiene el movimiento y prepara el ataque")] public float rangeToAttack;
    [Tooltip("Distancia en la que puede seguir al jugador desde su posición inicial")] public float rangeToFollowPlayer;
    [Tooltip("El peso determina su dificultad")] public int weight;
    public List<Shapes> goals = new List<Shapes>();
    [HideInInspector] public bool canTakeDamage = true;
    private Vector2 _initialPoint;
    [HideInInspector] public Vector2 detectPosition;
    private float _baseDelayToTakeDamage;

    [Header("Hit Reaction Settings")]
    public float hitFlashDuration = 1f;
    public float hitPauseDuration = 0.5f;
    public float knockbackIntensity = 5f;

    [Header("Private Content")]
    protected DrawMouse _target;
    private SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rb2d;
    private Color _originalColor;
    private bool isHit = false;
    [HideInInspector] public Room _myRoom;

    private void Start()
    {
        uiPosition = GameObject.Find("WorldSpace").transform;

        _rb2d = GetComponent<Rigidbody2D>();
        _target = FindAnyObjectByType<DrawMouse>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _originalColor = _spriteRenderer.color;

        _baseDelayToTakeDamage = delayToTakeDamage;
        _initialPoint = transform.position;

        #region BARS
        healthbar = Instantiate(healthbarPrefab, (transform.position + new Vector3(0, healthbarOffsetY, 0)), Quaternion.identity, uiPosition).GetComponent<Slider>();

        healthbar.maxValue = goals.Count;
        healthbar.value = healthbar.maxValue;

        damagebar = Instantiate(damagebarPrefab, (transform.position + new Vector3(0, damagebarOffsetY, 0)), Quaternion.identity, uiPosition).GetComponent<Slider>();

        damagebar.maxValue = delayToTakeDamage;
        damagebar.value = delayToTakeDamage;
        #endregion

        canTakeDamage = true;
    }
    private void Update()
    {
        if (isHit) return;

        if (goals.Count <= 0) DestroyElement();
        else
        {
            if (Vector3.Distance(transform.position, _target.transform.position) <= rangeToAttack)
            {
                if(detectPosition == Vector2.zero)
                {
                    detectPosition = _target.transform.position;
                    damagebar.gameObject.SetActive(true);
                }

                damagebar.value = delayToTakeDamage;
                delayToTakeDamage -= Time.deltaTime;

                if(delayToTakeDamage <= 0) { if (canTakeDamage) Attack(); }
            }

            if (Vector3.Distance((Vector3)_initialPoint, _target.transform.position) < rangeToFollowPlayer)
            {
                if (Vector3.Distance(transform.position, _target.transform.position) > rangeToAttack) { MoveToPlayer(); }
            }
        }

        // QUITAR BARS EN LEJANÍA
        if (healthbar != null && damagebar != null)
        {
            if (Vector2.Distance(transform.position, _target.transform.position) > rangeToAttack)
            {
                healthbar.gameObject.SetActive(false);
                damagebar.gameObject.SetActive(false);
            }
            else
            {
                healthbar.gameObject.SetActive(true);

                healthbar.transform.localPosition = transform.position + new Vector3(0, healthbarOffsetY, 0);
                damagebar.transform.localPosition = transform.position + new Vector3(0, damagebarOffsetY, 0);
            }
        }
    }
    private void Attack()
    {
        delayToTakeDamage = _baseDelayToTakeDamage;

        damagebar.gameObject.SetActive(false);

        LaunchAttack();
    }
    public abstract void LaunchAttack();
    public void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, speed * Time.deltaTime);
    }
    private void DestroyElement()
    {
        _myRoom.DestroyEnemy(this);
        Destroy(gameObject, 0.25f);
    }
    public void GetGoal()
    {
        if (goals.Count <= 0) return;

        if (_target.drawCompleteName == goals[0].ToString() && goals[0].ToString() != "")
        {
            TakeHit(_target.transform.position);
            canTakeDamage = false;

            if (goals.Count > 0) goals.RemoveAt(0);

            healthbar.value = goals.Count;

            Invoke("ResetDamage", delayToTakeDamage);
        }
    }
    private void ResetDamage() { canTakeDamage = true; }
    private void OnDestroy()
    {
        Destroy(healthbar.gameObject);
        Destroy(damagebar.gameObject);
    }
    /* ---------- HITS ---------- */
    public void TakeHit(Vector3 attackerPosition)
    {
        // Evitar múltiples reacciones al mismo tiempo
        if (isHit) return;

        isHit = true;
        StartCoroutine(HandleHitReaction(attackerPosition));
    }
    private IEnumerator HandleHitReaction(Vector3 attackerPosition)
    {
        // 1. Flash rojo
        StartCoroutine(FlashRed());

        // 2. Empuje
        Vector3 knockbackDirection = (transform.position - attackerPosition).normalized;
        if (_rb2d != null) _rb2d.AddForce(knockbackDirection * knockbackIntensity, ForceMode2D.Impulse);

        // 3. Pausa de movimiento
        yield return new WaitForSeconds(hitPauseDuration);

        // Permitir moverse nuevamente
        _rb2d.velocity = Vector2.zero;
        knockbackDirection *= 3f;
        isHit = false;
    }
    private IEnumerator FlashRed()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(hitFlashDuration);
        _spriteRenderer.color = _originalColor;
    }
    /* ---------- HITS ---------- */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { collision.GetComponent<PlayerController>().TakeDamage(damage); }
    }
}
