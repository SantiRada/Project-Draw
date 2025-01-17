using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Stats")]
    public float life;

    public void TakeDamage(float dmg)
    {
        if(dmg > life) { life -= dmg; }
        else
        {
            if(life > 1) { life = 1; }
            else { life = 0; }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DamageSector"))
        {
            string[] divisor = collision.gameObject.name.Split('-');
            float damage = float.Parse(divisor[1]);

            TakeDamage(damage);
        }
    }
}