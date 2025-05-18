using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [SerializeField] private GridSystem gridSetup;
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private BusInitializer busInitializer;
    [SerializeField] private WallSpawner wallSpawner;
    [SerializeField] private PassengerSpawner passengerSpawner;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform busParent;
    [SerializeField] private Transform wallsParent;

    private void Start()
    {

        GenerateGridWithBuses();
    }

    [ContextMenu("Generate grid")]
    public void GenerateGridWithBuses()
    {
        ClearGrid();
        gridGenerator.Generate(gridSetup);
        busInitializer.SpawnBuses(gridSetup,passengerSpawner);
        wallSpawner.PlaceWalls(gridSetup);
    }


    public GameObject GetCellFromIndex(int index)
    {
       return gridGenerator.GetCellFromGrid(index);
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
}