using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProceduralMap", menuName = "ScriptableObjects/ProceduralMap")]
public class ProceduralMap : ScriptableObject {

    [Header("Enemies Data")]
    public List<Enemy> typeEnemies = new List<Enemy>();
    public int totalWeight;
    public float difficultyMultiplier;

    [Header("Scene Data")]
    public bool isPauseToInit;
    public bool initialWithCine;
    public Room specificRoom = null;

    [Header("Dialogues")]
    public Dialogue dialogue;
}

public class Dialogue : MonoBehaviour {

    [Header("UI Content")]
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textMessage;

    [Header("Data Content")]
    [HideInInspector] public string nameAuthor;
    [HideInInspector] public string message;

    public bool dissapearWithClic;
    public KeyCode clicPerDisappear;
}
