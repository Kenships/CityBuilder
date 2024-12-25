using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Building : MonoBehaviour, ISerializationCallbackReceiver
{
    
    public const int ROW_MAX = 5;
    public const int COL_MAX = 5;
    public bool[,] _configuration = new bool[ROW_MAX, COL_MAX]; 
    
    [SerializeField, HideInInspector]
    private bool[] _serializedConfiguration;
    
    
    private List<Vector2Int> _occupancyCells;
    
    private void Start()
    {
        _occupancyCells = new List<Vector2Int>();
        for (int row = 0; row < ROW_MAX; row++)
        {
            for (int col = 0; col < COL_MAX; col++)
            {
                if (_configuration[row, col])
                {
                    _occupancyCells.Add(new Vector2Int(row, col));
                }
            }
        }
        Invoke(nameof(Print), 0);
    }

    private void Print()
    {
        foreach (Vector2Int occupancyCell in _occupancyCells)
        {
            Debug.Log(occupancyCell.ToString());
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(0,90,0);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(0,-90,0);
        }
    }
    //rotation in degrees
    public List<Vector2Int> GetOccupancyCells()
    {
        int rotation = (int) transform.rotation.eulerAngles.z;
        
        if(rotation == 0) return  _occupancyCells;
        List<Vector2Int> rotatedVectors = new List<Vector2Int>();
        for (int rotationCount = 0; rotationCount < rotation/90; rotationCount++)
        {
            foreach (Vector2Int vector in _occupancyCells)
            {
                rotatedVectors.Add(RotateClockwise(vector));
            }
        }
        return rotatedVectors;
    }
    
    private Vector2Int RotateClockwise(Vector2Int vector)
    {
        return new Vector2Int(-vector.y, vector.x);
    }
    
    public void OnBeforeSerialize()
    {
        // Make sure our hidden array is the right size
        if (_serializedConfiguration == null || _serializedConfiguration.Length != ROW_MAX * COL_MAX)
        {
            _serializedConfiguration = new bool[ROW_MAX * COL_MAX];
        }

        // Copy from our 2D array into the 1D array
        for (int row = 0; row < ROW_MAX; row++)
        {
            for (int col = 0; col < COL_MAX; col++)
            {
                int index = row * COL_MAX + col;
                _serializedConfiguration[index] = _configuration[row, col];
            }
        }
    }

    public void OnAfterDeserialize()
    {
        // If we do not have a valid 1D array, create one anyway
        if (_serializedConfiguration == null || _serializedConfiguration.Length != ROW_MAX * COL_MAX)
        {
            _serializedConfiguration = new bool[ROW_MAX * COL_MAX];
        }

        // Rebuild the 2D array from the 1D array
        _configuration = new bool[ROW_MAX, COL_MAX];
        for (int row = 0; row < ROW_MAX; row++)
        {
            for (int col = 0; col < COL_MAX; col++)
            {
                int index = row * COL_MAX + col;
                _configuration[row, col] = _serializedConfiguration[index];
            }
        }
    }
}
