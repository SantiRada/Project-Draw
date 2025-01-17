using System.Collections;
using UnityEngine;

public class GeneratorRoom : MonoBehaviour {

    [Header("Rooms")]
    public GameObject[] room;
    private Room currentRoom;
    private PlayerMovement _player;

    [Header("UI Content")]
    public float delayBlackScreen;
    public Animator blackScreen;

    private ProgressManager _progressManager;
    private CameraManager _camera;

    private void Start()
    {
        _camera = FindAnyObjectByType<CameraManager>();
        _player = FindAnyObjectByType<PlayerMovement>();
        _progressManager = FindAnyObjectByType<ProgressManager>();

        CreateRoom(false);
    }
    public void CreateRoom(bool dir)
    {
        // Actualización de estado de progreso entre rooms
        if(dir) { _progressManager.NextScene(); }

        StartCoroutine("CreateNewRoom");
    }
    private IEnumerator CreateNewRoom()
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.SetBool("Loading", true);

        yield return new WaitForSeconds(delayBlackScreen / 2);

        _player.canMove = false;

        if (currentRoom != null) { Destroy(currentRoom.gameObject); }

        if (_progressManager.rooms[_progressManager.currentRoom].specificRoom != null)
        {
            currentRoom = Instantiate(_progressManager.rooms[_progressManager.currentRoom].specificRoom.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Room>();
        }
        else
        {
            currentRoom = Instantiate(room[Random.Range(0, room.Length)].gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Room>();
        }
        currentRoom.transform.localPosition = Vector3.zero;

        // ESTABLECER POSICIONAMIENTO DE LA CÁMARA
        _camera.SetPosition(currentRoom.minPosCamera, currentRoom.maxPosCamera);

        _player.transform.position = currentRoom.initialPos.position;

        blackScreen.SetBool("Loading", false);
        blackScreen.gameObject.SetActive(false);

        yield return new WaitForSeconds(delayBlackScreen / 2);

        _player.canMove = true;
    }
}