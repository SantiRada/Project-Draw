using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour {

    public int countSpells = 3;
    public Door finalDoor;
    public List<Door> doorList = new List<Door>();
    [Space]
    private int currentCountSpells = 3;
    private bool canVerify = true;

    private DrawMouse _drawer;

    private void Start()
    {
        _drawer = FindAnyObjectByType<DrawMouse>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canVerify)
        {
            if (_drawer.drawCompleteName == "") return;

            canVerify = false;

            for (int i = 0; i < doorList.Count; i++)
            {
                doorList[i].VerifySpell(_drawer);
            }

            currentCountSpells--;

            finalDoor.VerifySpell(_drawer);

            if (currentCountSpells < 0)
            {
                currentCountSpells = countSpells;

                for (int i = 0; i < doorList.Count; i++) { doorList[i].RestartDoor(); }
            }
            else if (currentCountSpells == 0)
            {
                int countOpened = 0;

                for (int i = 0; i < doorList.Count; i++)
                {
                    if (doorList[i].isOpen)
                    {
                        countOpened++;
                    }
                }

                if (countOpened >= doorList.Count) finalDoor.OpenDoor();
            }

            Invoke("ResetVerify", 1f);
        }
    }
    private void ResetVerify()
    {
        _drawer.drawCompleteName = "";
        canVerify = true;
    }
}