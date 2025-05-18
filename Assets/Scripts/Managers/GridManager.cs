using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private GridSystem gridSetup;
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private BusInitializer busInitializer;
    [SerializeField] private WallSpawner wallSpawner;
    [SerializeField] private PassengerSpawner passengerSpawner;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform busParent;
    [SerializeField] private Transform wallsParent;

    private List<int> occupiedCells = new List<int>();
    private Dictionary<int, GameObject> gridCells = new();

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {

        GenerateGridWithBuses();
    }

    [ContextMenu("Generate grid")]
    public void GenerateGridWithBuses()
    {
        ClearGrid();
        gridGenerator.Generate(gridSetup);
        busInitializer.SpawnBuses(gridSetup, passengerSpawner);
        wallSpawner.PlaceWalls(gridSetup);
    }

    public void StoreGridCellToIndex(int index, GameObject cell)
    {
        gridCells.Add(index, cell);
    }

    public GameObject GetCellFromIndex(int index)
    {
        if (gridCells.TryGetValue(index, out GameObject cell))
            return cell;

        return null;
    }

    public int GetCellIndexByWorldPos(Vector3 worldPos)
    {
        Vector3 relativePos = worldPos - gridSetup.originPosition;

        int x = Mathf.RoundToInt(relativePos.x);
        int y = Mathf.RoundToInt(relativePos.z);

        return y * gridSetup.columns + x;
    }

    public void AssignGridCell(int cellIndex)
    {
        occupiedCells.Add(cellIndex);
    }

    public void AssignGridCell(Vector3 worldPos)
    {
        int index = GetCellIndexByWorldPos(worldPos);
        AssignGridCell(index);
    }

    public void FreeGridCell(int cellIndex)
    {
        occupiedCells.Remove(cellIndex);
    }

    public void FreeGridCell(Vector3 worldPos)
    {
        int index = GetCellIndexByWorldPos(worldPos);
        FreeGridCell(index);
    }

    [ContextMenu("Clear Grid and Buses")]
    private void ClearGrid()
    {
        ClearChildren(tileParent);
        ClearChildren(busParent);
        ClearChildren(wallsParent);
    }


    private void ClearChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            if (child != parent)
                DestroyImmediate(child.gameObject);
        }
    }

    internal bool IsCellOccupied(Vector3 targetPos)
    {
        int index = GetCellIndexByWorldPos(targetPos);
        return occupiedCells.Contains(index);
    }
}