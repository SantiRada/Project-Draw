using UnityEngine;

public class CameraManager : MonoBehaviour {

    public float speedMovement;
    private Vector2 minPos, maxPos;
    private Transform _target;

    private void Start()
    {
        _target = FindAnyObjectByType<PlayerMovement>().transform;
    }
    public void SetPosition(Vector2 min, Vector2 max)
    {
        minPos = min;
        maxPos = max;
    }
    private void FixedUpdate()
    {
        float posX = Mathf.Clamp(_target.position.x, minPos.x, maxPos.x);
        float posY = Mathf.Clamp(_target.position.y, minPos.y, maxPos.y);
        Vector3 newPosition = new Vector3(posX, posY, -10);

        transform.position = Vector3.Lerp(transform.position, newPosition, speedMovement * Time.fixedDeltaTime);
    }
}