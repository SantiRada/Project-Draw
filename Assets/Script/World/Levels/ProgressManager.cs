using UnityEngine;

public class ProgressManager : MonoBehaviour {

    [Header("Content")]
    public ProceduralMap[] rooms;
    [HideInInspector] public int currentRoom = 0;

    private void Start() { currentRoom = 0; }
    public ProceduralMap GetMap() { return rooms[currentRoom]; }
    public void NextScene()
    {
        currentRoom++;

        if (currentRoom >= rooms.Length) { Debug.Log("Has llegado al limite"); }
    }
}