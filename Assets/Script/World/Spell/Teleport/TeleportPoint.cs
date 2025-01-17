using UnityEngine;

public class TeleportPoint : MonoBehaviour {

    public int idPoint;
    public int connectPoint;

    [HideInInspector] public TeleportManager manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DrawMouse drawContent = collision.GetComponent<DrawMouse>();
            if (drawContent.drawCompleteName == manager.spellNecessary.ToString())
            {
                drawContent.drawCompleteName = "";
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DrawMouse drawContent = collision.GetComponent<DrawMouse>();
            if (drawContent.drawCompleteName == manager.spellNecessary.ToString())
            {
                drawContent.drawCompleteName = "";
                collision.transform.position = manager.GetPoint(connectPoint).transform.position;
            }
        }
    }
}
