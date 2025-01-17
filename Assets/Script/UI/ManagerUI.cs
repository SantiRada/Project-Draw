using UnityEngine;

public class ManagerUI : MonoBehaviour {

    public GameObject sectorBook;
    public GameObject sectorMovement;
    public GameObject sectorDraw;
    public GameObject sectorMenu;
    [Space]
    public GameObject leftZone, rightZone;

    private DrawMouse drawMouse;

    private void Awake()
    {
        drawMouse = FindAnyObjectByType<DrawMouse>();

        leftZone.SetActive(false);
        rightZone.SetActive(false);
    }
    private void Start()
    {
        sectorDraw.SetActive(false);
        sectorBook.SetActive(false);

        ChangeModeTo(PlayerPrefs.GetInt("ModeInput", 1));
    }
    public void OpenMenu()
    {
        sectorMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void OpenBook()
    {
        sectorBook.SetActive(true);

        sectorMovement.SetActive(false);
        sectorDraw.SetActive(false);
    }
    public void OpenDraw()
    {
        sectorDraw.SetActive(true);

        sectorBook.SetActive(false);
        sectorMovement.SetActive(false);

        drawMouse.canDraw = true;
    }
    public void GoToMovement()
    {
        sectorMovement.SetActive(true);

        sectorBook.SetActive(false);
        sectorDraw.SetActive(false);
        sectorMenu.SetActive(false);

        drawMouse.canDraw = false;
        Time.timeScale = 1f;
    }
    public void ChangeModeTo(int i)
    {
        if(i == 0)
        {
            leftZone.SetActive(true);
            rightZone.SetActive(false);
        }
        else
        {
            leftZone.SetActive(false);
            rightZone.SetActive(true);
        }

        PlayerPrefs.SetInt("ModeInput", i);
    }
}
