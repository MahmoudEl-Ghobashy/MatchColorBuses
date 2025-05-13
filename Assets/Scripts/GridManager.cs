using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] GridSystem gridSetup;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject holePrefab;
    [SerializeField] Transform tileParent;

    [Header("Bus segments")] [SerializeField]
    private Transform busParent;

    [SerializeField] private BusSpawner bus;

    [Header("Color materials")] [SerializeField]
    private Material redMaterial;

    [SerializeField] private Material redDullMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material blueDullMaterial;
    [SerializeField] private Material purpleMaterial;
    [SerializeField] private Material purpleDullMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material greenDullMaterial;
    [SerializeField] private Material orangeMaterial;
    [SerializeField] private Material orangeDullMaterial;


    private void Start()
    {
        GenerateGrid();
        SetBuses();
    }

    private void GenerateGrid()
    {
        for (int y = 0; y < gridSetup.rows; y++)
        {
            for (int x = 0; x < gridSetup.columns; x++)
            {
                if (gridSetup.holeIndex.Contains(y * gridSetup.columns + x))
                {
                    GenerateHole(x, y);
                    continue;
                }

                if (gridSetup.obstacleIndex.Contains(y * gridSetup.columns + x))
                {
                    GenerateObstacle(x, y);
                    continue;
                }

                Vector3 worldPos = gridSetup.originPosition +
                                   new Vector3(x, 0f, y);

                GameObject tile = Instantiate(tilePrefab, worldPos, tilePrefab.transform.rotation, tileParent);
                tile.name = $"grid{x}_{y}";
            }
        }
    }

    private void GenerateObstacle(int x, int y)
    {
        Vector3 worldPos = gridSetup.originPosition +
                           new Vector3(x, 0f, y);

        GameObject obs = Instantiate(obstaclePrefab, worldPos, obstaclePrefab.transform.rotation, tileParent);
        obs.name = $"obs{x}_{y}";
    }

    private void GenerateHole(int x, int y)
    {
        Vector3 worldPos = gridSetup.originPosition +
                           new Vector3(x, 0f, y);

        GameObject hole = Instantiate(holePrefab, worldPos, holePrefab.transform.rotation, tileParent);
        hole.name = $"hole{x}_{y}";
    }

    private void SetBuses()
    {
        if (gridSetup.hasBlueBus)
        {
            BusSpawner blueBus = Instantiate(bus, busParent);
            blueBus.SetMaterialColors(blueMaterial, blueDullMaterial);
            blueBus.SetBusPositions(CalculateBusPositions(gridSetup.blueBusPositions));
        }

        if (gridSetup.hasGreenBus)
        {
            BusSpawner greenBus = Instantiate(bus, busParent);
            greenBus.SetMaterialColors(greenMaterial, greenDullMaterial);
            greenBus.SetBusPositions(CalculateBusPositions(gridSetup.greenBusPositions));
        }

        if (gridSetup.hasOrangeBus)
        {
            BusSpawner orangeBus = Instantiate(bus, busParent);
            orangeBus.SetMaterialColors(orangeMaterial, orangeDullMaterial);
            orangeBus.SetBusPositions(CalculateBusPositions(gridSetup.orangeBusPositions));
        }

        if (gridSetup.hasPurpleBus)
        {
            BusSpawner purpleBus = Instantiate(bus, busParent);
            purpleBus.SetMaterialColors(purpleMaterial, purpleDullMaterial);
            purpleBus.SetBusPositions(CalculateBusPositions(gridSetup.purpleBusPositions));
        }

        if (gridSetup.hasRedBus)
        {
            BusSpawner redBus = Instantiate(bus, busParent);
            redBus.SetMaterialColors(redMaterial, redDullMaterial);
            redBus.SetBusPositions(CalculateBusPositions(gridSetup.redBusPositions));
        }
    }

    private List<Vector3> CalculateBusPositions(List<int> gridPositions)
    {
        List<Vector3> worldPositions = new List<Vector3>();

        for (int i = 0; i < gridPositions.Count; i++)
        {
            int gridPos = gridPositions[i];
            int x = gridPos % gridSetup.columns;
            int y = gridPos / gridSetup.columns;
            worldPositions.Add(gridSetup.originPosition + new Vector3(x, 0f, y));
        }

        return worldPositions;
    }
}