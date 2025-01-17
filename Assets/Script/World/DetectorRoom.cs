using UnityEngine;

public class DetectorRoom : MonoBehaviour {

    private GeneratorRoom _generator;
    [HideInInspector] public Room _currentRoom;

    private void Start() { _generator = FindAnyObjectByType<GeneratorRoom>(); }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_currentRoom.canAdvance)
            {
                _generator.CreateRoom(true);
                collision.GetComponent<PlayerMovement>().SetPoture(100);
            }
        }
    }
}