using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] private Transform tileSelectVisual;
    
    private Vector3Int _lastGridPosition;

    private void Update()
    {
        Vector3Int currentGridPosition = GetGridPosition();
        if (_lastGridPosition != currentGridPosition)
        {
            _lastGridPosition = currentGridPosition;
            Debug.Log(currentGridPosition);
        }
        tileSelectVisual.position = currentGridPosition;
    }
    private Vector3Int GetGridPosition()
    {
        bool isNull = !InputManager.Instance.TryGetMousePosition(out var mousePosition);
        return isNull ? _lastGridPosition : grid.WorldToCell(mousePosition);
    }
}
