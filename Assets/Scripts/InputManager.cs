using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform mousePointer;
    
    //Currently set to Default
    [SerializeField] private LayerMask mouseRaycastLayer;
    
    private InputSystem_Actions _inputSystem;
    private Vector3 _lastMousePosition;
    private void Awake()
    {
        Instance = this;
        _inputSystem = new InputSystem_Actions();
        _inputSystem.Enable();
    }

    private void Update()
    {
        TryGetMousePosition(out var mousePosition);
        mousePointer.position = mousePosition;
    }

    public Vector3 GetMouseDragDirNormalized(int mouseButton)
    {
        if (!TryGetMousePosition(out var currentPosition))
        {
            return Vector3.zero;
        }
        if (Input.GetMouseButtonDown (mouseButton))
        {
            _lastMousePosition = currentPosition;
        }

        if (Input.GetMouseButton(mouseButton))
        {
            Vector3 positionDelta = _lastMousePosition - currentPosition;
            return positionDelta.magnitude < 0.1f ? Vector3.zero : positionDelta.normalized;
        }
        
        return Vector3.zero;
    }

    public bool TryGetMousePosition(out Vector3 worldMousePosition)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        worldMousePosition = default;
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, mouseRaycastLayer))
        {
            worldMousePosition = hit.point;
            return true;
        }
        return false;
    }

}
