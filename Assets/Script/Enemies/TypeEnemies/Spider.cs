using UnityEngine;

public class Spider : Enemy {

    public GameObject spiderweb;
    public float delayToDestroyWeb = 1.5f;

    public override void LaunchAttack()
    {
        GameObject web = Instantiate(spiderweb, detectPosition, Quaternion.identity);
        web.name = "Spiderweb-" + damage;
        Destroy(web, delayToDestroyWeb);

        detectPosition = Vector2.zero;
    }
}