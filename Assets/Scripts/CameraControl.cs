using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private const int LEFT_MB = 1;
    [SerializeField] private float panSpeed;
    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 movementDir = InputManager.Instance.GetMouseDragDirNormalized(LEFT_MB);
        transform.position += movementDir * panSpeed * Time.deltaTime;
    }
}
