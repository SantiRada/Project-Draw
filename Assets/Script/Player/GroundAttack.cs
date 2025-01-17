using System.Collections;
using UnityEngine;

public class GroundAttack : MonoBehaviour {

    [Header("Stats")]
    public float timeToDissappear;
    public Vector3[] countSizeToShapes;
    private string lastShape;
    private int currentSize = 0;

    [Header("Visual Content")]
    public SpriteRenderer shapeRenderer;
    public Sprite[] listShapes;
    private SpriteRenderer _myRenderer;
    private Collider2D _myCollider;

    private void Awake()
    {
        _myRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        _myCollider = GetComponent<Collider2D>();

        _myRenderer.enabled = false;
        shapeRenderer.enabled = false;
    }
    public void ShowAttack(string data, Color color)
    {
        if(lastShape != null)
        {
            if(lastShape == data)
            {
                currentSize++;

                if (currentSize >= countSizeToShapes.Length) { currentSize = (countSizeToShapes.Length - 1); }

            } else { currentSize = 0; }

            transform.localScale = countSizeToShapes[currentSize];
        } else
        {
            transform.localScale = countSizeToShapes[0];
        }

        lastShape = data;

        Sprite spr = shapeRenderer.sprite;
        switch (data)
        {
            case "MayorQue": spr = listShapes[0]; break;
            case "MenorQue": spr = listShapes[1]; break;
            case "Z": spr = listShapes[2]; break;
            case "N": spr = listShapes[3]; break;
            case "I": spr = listShapes[4]; break;
            case "U": spr = listShapes[5]; break;
            case "UInvertida": spr = listShapes[6]; break;
        }

        shapeRenderer.color = new Color(color.r, color.g, color.b, 0.5f);
        _myRenderer.color = new Color(color.r, color.g, color.b, 0.15f);

        shapeRenderer.sprite = spr;

        StopCoroutine("Attacking");
        StartCoroutine("Attacking");
    }
    private IEnumerator Attacking()
    {
        _myCollider.enabled = true;

        _myRenderer.enabled = true;
        shapeRenderer.enabled = true;

        yield return new WaitForSeconds(timeToDissappear);
        
        _myCollider.enabled = false;

        shapeRenderer.enabled = false;
        _myRenderer.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy.canTakeDamage) { enemy.GetGoal(); }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy.canTakeDamage) { enemy.GetGoal(); }
        }
    }
}