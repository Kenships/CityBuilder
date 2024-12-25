using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private int gridRows, gridCols;
    [SerializeField] Grid grid;
    [SerializeField] private Transform tileSelectVisual;
    [SerializeField] private GameObject testUnit1, testUnit2, testUnit3;
    [SerializeField] private Vector2Int gridStart;
    private Vector3Int _lastGridPosition;
    private GridUnit[,] _gridUnits;
    private GameObject _heldObject;
    
    private void Awake()
    {
        _gridUnits = new GridUnit[gridRows, gridCols];
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridCols; col++)
            {
                _gridUnits[row, col] = new GridUnit();
            }
        }
    }
    private void Update()
    {
        Vector3Int currentGridPosition = GetGridPosition();
        
        _lastGridPosition = currentGridPosition;
        
        tileSelectVisual.position = currentGridPosition;
        if (_heldObject != null)
        {
            _heldObject.transform.position = currentGridPosition;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _heldObject = Instantiate(testUnit1, tileSelectVisual.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _heldObject = Instantiate(testUnit2, tileSelectVisual.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _heldObject = Instantiate(testUnit3, tileSelectVisual.position, Quaternion.identity);
        }

        if (_heldObject != null )
        {
            Building building = _heldObject.GetComponent<Building>();
            if (Input.GetKeyDown(KeyCode.Mouse0) && IsValidPosition(building.GetOccupancyCells()))
            {
                OccupyArea(building.GetOccupancyCells());
                _heldObject = null;
            }
        }
    }

    private void OccupyArea(List<Vector2Int> getOccupancyCells)
    {
        foreach (Vector2Int cell in getOccupancyCells)
        {
            Vector2Int localGridPosition = GetLocalGridPosition(new Vector2Int(_lastGridPosition.x , _lastGridPosition.z));
            Vector2Int currentPosition = localGridPosition + cell;
            _gridUnits[currentPosition.x, currentPosition.y].Occupy();
        }
    }

    private bool IsValidPosition(List<Vector2Int> gridOccupancy)
    {
        
        foreach (var cell in gridOccupancy)
        {
            
            Vector2Int localGridPosition = GetLocalGridPosition(new Vector2Int(_lastGridPosition.x , _lastGridPosition.z));
            Vector2Int currentPosition = localGridPosition + cell;
            Debug.Log(currentPosition);
            if (currentPosition.magnitude < 0
                || currentPosition.x >= gridRows
                || currentPosition.y >= gridCols
                || _gridUnits[currentPosition.x, currentPosition.y].Occupied)
            {
                Debug.Log("Invalid grid position");
                return false;
            }
        }
        return true;
    }
    private Vector3Int GetGridPosition()
    {
        bool isNull = !InputManager.Instance.TryGetMousePosition(out var mousePosition);
        return isNull ? _lastGridPosition : grid.WorldToCell(mousePosition);
    }

    private Vector2Int GetLocalGridPosition(Vector2Int worldGridPosition)
    {
        return worldGridPosition - gridStart;
    }
}
