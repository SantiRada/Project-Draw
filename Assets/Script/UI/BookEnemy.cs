using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookEnemy : MonoBehaviour {

    [Header("List Content")]
    public Enemy[] listEnemies;

    [Header("UI Content")]
    public GameObject _books;
    public Image[] imagesOfEnemies;
    public TextMeshProUGUI[] textOfEnemies;

    [HideInInspector] public bool isOpen;
    
    private void Start()
    {
        _books.gameObject.SetActive(false);
    }
    public void OpenBook()
    {
        _books.gameObject.SetActive(true);
    }
    public void CloseBook()
    {
        _books.gameObject.SetActive(false);
    }
    private string CalculateSpell(List<Shapes> shapes)
    {
        string value = "";

        foreach (Shapes shape in shapes)
        {
            switch (shape)
            {
                case Shapes.UInvertida: value += "∩"; break;
                case Shapes.MayorQue: value += ">"; break;
                case Shapes.MenorQue: value += "<"; break;
                default: value += shape.ToString(); break;
            }
        }

        return value;
    }
}