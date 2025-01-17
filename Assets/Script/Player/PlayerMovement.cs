using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    [Header("Scene Content")]
    [HideInInspector] public GroundAttack groundAttack;

    [Header("Movement")]
    public float speed = 5f;
    public float moveCooldown;
    public LayerMask collisionableLayer;
    [Space]
    [HideInInspector] public bool canMove;
    private Vector2 _movement;
    private bool _isHolding = false;
    private bool _isMoving = false;
    private float _baseMoveCooldown;
    private Vector3 _targetPosition;

    [Header("Poture")]
    public Slider potureBar;
    public int poture;
    public int poturePerAttack;
    public int poturePerSecond;

    [Header("Private Content")]
    private Rigidbody2D _rb2d;
    private SpriteRenderer _spr;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
        groundAttack = GetComponentInChildren<GroundAttack>();

        _baseMoveCooldown = moveCooldown;
        moveCooldown = 0;

        potureBar.maxValue = poture;
        potureBar.value = poture;
        _targetPosition = transform.position;
    }
    private void Update()
    {
        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
            {
                transform.position = _targetPosition;
                _isMoving = false;

                moveCooldown = _baseMoveCooldown;
            }
        }

        if (_isHolding)
        {
            moveCooldown -= Time.deltaTime;

            if (moveCooldown <= 0)
            {
                AttemptMove();
            }
        }
    }
    public void MoveInDirection(int dir)
    {
        _isHolding = true;

        switch (dir)
        {
            case 0:
                _movement = new Vector2(-0.5f, 0);
                _spr.flipX = true;
                break;  // LEFT
            case 1:
                _movement = new Vector2(0.5f, 0);
                _spr.flipX = false;
                break;   // RIGHT
            case 2: _movement = new Vector2(0, 0.5f); break;   // TOP
            case 3: _movement = new Vector2(0, -0.5f); break;  // BOTTOM
        }
    }
    public void CancelMove()
    {
        moveCooldown = 0;
        _isMoving = false;
        _isHolding = false;

        transform.position = _targetPosition;

        _movement = Vector2.zero;
        _rb2d.velocity = Vector2.zero;
    }
    private void AttemptMove()
    {
        Vector3 nextPosition = transform.position + (Vector3)_movement;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, _movement, 1f, collisionableLayer);

        if (hit.collider == null)
        {
            _targetPosition = nextPosition;
            _isMoving = true;
        }
    }
    public void Attack(string data, Color color)
    {
        if (poture < poturePerAttack) return;

        SetPoture(-poturePerAttack);

        groundAttack.ShowAttack(data, color);
    }
    public void SetPoture(int value)
    {
        if (value >= (int)potureBar.maxValue) { poture = (int)potureBar.maxValue; }
        else { poture += value; }

        if (poture < 0) poture = 0;

        potureBar.value = poture;
    }
}