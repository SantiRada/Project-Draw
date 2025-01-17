using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool inSystem = false;
    public List<Shapes> keys = new List<Shapes>();
    
    [Header("Private Content")]
    [HideInInspector] public List<Shapes> descKeys = new List<Shapes>();
    public bool isOpen = false;
    public Collider2D limitDoor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<DrawMouse>().drawCompleteName = "";
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DrawMouse drawer = collision.gameObject.GetComponent<DrawMouse>();

            if(drawer.drawCompleteName != "") VerifySpell(drawer);
        }
    }
    public void VerifySpell(DrawMouse drawer, bool revistionSystem = false)
    {
        if (keys.Count <= 0) return;

        if (drawer.drawCompleteName == keys[0].ToString() || inSystem)
        {
            if (inSystem && !isOpen)
            {
                Debug.Log("Review System Void...");
                return;
            }

            descKeys.Add(keys[0]);
            keys.RemoveAt(0);

            if (keys.Count <= 0) OpenDoor();
        }
    }
    public void RestartDoor()
    {
        limitDoor.gameObject.SetActive(true);
        isOpen = false;

        List<Shapes> newKeys = new List<Shapes>();

        if(descKeys.Count > 0) newKeys.AddRange(descKeys);
        if (keys.Count > 0) newKeys.AddRange(keys);

        keys = newKeys;

        descKeys.Clear();
    }
    public void OpenDoor()
    {
        isOpen = true;
        limitDoor.gameObject.SetActive(false);
    }
}
