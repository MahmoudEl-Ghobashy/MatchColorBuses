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

    [Header("Walls")] [SerializeField] GameObject wallStraightPrefab;
    [SerializeField] private GameObject wallEdgePrefab;
    [SerializeField] private Transform wallsParent;

    [Header("Passengers")]
    [SerializeField] GameObject passengerPrefab;
    [SerializeField] GameObject passengerGate;

    private int _cols;
    private int _rows;

    private void Start()
    {
        GenerateGridWithBuses();
    }

    [ContextMenu("Generate Grid With Buses")]
    private void GenerateGridWithBuses()
    {
        _cols = gridSetup.columns;
        _rows = gridSetup.rows;
        ClearGrid();
        GenerateGrid();
        SetBuses();
        SetWalls();
    }

    private void GenerateGrid()
    {
        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _cols; x++)
            {
                if (gridSetup.holeIndex.Contains(y * _cols + x))
                {
                    GenerateHole(x, y);
                    continue;
                }

                if (gridSetup.obstacleIndex.Contains(y * _cols + x))
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
            SetPassage(gridSetup.bluePassagePosition, blueMaterial,gridSetup.blueBusPositions.Count);
        }

        if (gridSetup.hasGreenBus)
        {
            BusSpawner greenBus = Instantiate(bus, busParent);
            greenBus.SetMaterialColors(greenMaterial, greenDullMaterial);
            greenBus.SetBusPositions(CalculateBusPositions(gridSetup.greenBusPositions));
            SetPassage(gridSetup.greenPassagePosition, greenMaterial,   gridSetup.greenPassagePosition);
        }

        if (gridSetup.hasOrangeBus)
        {
            BusSpawner orangeBus = Instantiate(bus, busParent);
            orangeBus.SetMaterialColors(orangeMaterial, orangeDullMaterial);
            orangeBus.SetBusPositions(CalculateBusPositions(gridSetup.orangeBusPositions));
            SetPassage(gridSetup.orangePassagePosition, orangeMaterial,gridSetup.orangePassagePosition);
        }

        if (gridSetup.hasPurpleBus)
        {
            BusSpawner purpleBus = Instantiate(bus, busParent);
            purpleBus.SetMaterialColors(purpleMaterial, purpleDullMaterial);
            purpleBus.SetBusPositions(CalculateBusPositions(gridSetup.purpleBusPositions));
            SetPassage(gridSetup.purplePassagePosition, purpleMaterial,gridSetup.purplePassagePosition);
        }

        if (gridSetup.hasRedBus)
        {
            BusSpawner redBus = Instantiate(bus, busParent);
            redBus.SetMaterialColors(redMaterial, redDullMaterial);
            redBus.SetBusPositions(CalculateBusPositions(gridSetup.redBusPositions));
            SetPassage(gridSetup.redPassagePosition, redMaterial,gridSetup.redPassagePosition);
        }
    }

    private void SetPassage(int passagePosition, Material material,int lengthOfBus)
    {
        Vector3 direction;

        GameObject passage = Instantiate(passengerGate, wallsParent);
        int y = passagePosition / _cols;
        int x = passagePosition % _cols;
        if (y == 0)
        {
            passage.transform.position = gridSetup.originPosition + new Vector3(x, 0, y - 0.6f);
            passage.transform.rotation = Quaternion.Euler(0, 0, 0);
            direction = Vector3.back;
        }
        else if (y == _rows - 1)
        {
            passage.transform.position = gridSetup.originPosition + new Vector3(x, 0, y + 0.6f);
            passage.transform.rotation = Quaternion.Euler(0, 180, 0);
           direction = Vector3.forward;
        }
        else if (x == 0)
        {
            passage.transform.position = gridSetup.originPosition + new Vector3(x -0.6f, 0, y);
            passage.transform.rotation = Quaternion.Euler(0, 90, 0);
            direction = Vector3.left;
        }
        else if(x == _cols - 1)
        {
            passage.transform.position = gridSetup.originPosition + new Vector3(x + 0.6f, 0, y);
            passage.transform.rotation = Quaternion.Euler(0, 270, 0);
            direction = Vector3.right;
        }
        else
        {
            DestroyImmediate(passage);
            return;
        }

        Material[] materials = passage.GetComponent<MeshRenderer>().sharedMaterials;

        materials[0] = material;
        passage.GetComponent<MeshRenderer>().sharedMaterials = materials;
        SpawnPassengersAtPassage(passage.transform.position, direction, 4 * lengthOfBus, material);
    }

    private void SpawnPassengersAtPassage(Vector3 passagePosition, Vector3 direction, int count,Material color)
    {
        float forwardSpacing = 0.4f;
        float sideOffset = 0.15f;

        Vector3 perpendicular = Vector3.Cross(Vector3.up, direction).normalized;

        for (int i = 1; i < count + 1; i++)
        {
            Vector3 forwardOffset = direction * (i * forwardSpacing);
            float sideDir = (i % 2 == 0) ? 1f : -1f;
            Vector3 side = perpendicular * sideOffset * sideDir;

            Vector3 finalPosition = passagePosition + forwardOffset + side;
            GameObject passenger =  Instantiate(passengerPrefab, finalPosition, Quaternion.identity, wallsParent);
            if (direction == Vector3.left || direction == Vector3.right)
            {
                passenger.transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            passenger.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = color;
        }
    }

    private List<Vector3> CalculateBusPositions(List<int> gridPositions)
    {
        List<Vector3> worldPositions = new List<Vector3>();

        for (int i = 0; i < gridPositions.Count; i++)
        {
            int gridPos = gridPositions[i];
            int y = gridPos / _cols;
            int x = gridPos % _cols;
            worldPositions.Add(gridSetup.originPosition + new Vector3(x, 0f, y));
        }

        return worldPositions;
    }

    private void SetWalls()
    {
        //0.6 is spacing from bottom and right side
        //0.4 is spacing from top and left side
        
        //Set Edges
        PlaceWallEdge(-0.6f, -0.6f, 270);
        PlaceWallEdge(_cols - 0.4f, -0.6f, 180);
        PlaceWallEdge(_cols - 0.4f, _rows - 0.4f, 90);
        PlaceWallEdge(-0.6f, _rows - 0.4f, 0);
        //Set walls
        for (int i = 0; i < _cols; i++)
        {
            PlaceWallStraight(i, -0.6f, 90);
            PlaceWallStraight(i, _rows - 0.4f, 90);
        }

        for (int i = 0; i < _rows; i++)
        {
            PlaceWallStraight(-0.6f, i, 0);
            PlaceWallStraight(_cols - 0.4f, i, 0);
        }
    }

    private void PlaceWallEdge(float x, float y, float yRotation)
    {
        Vector3 pos = gridSetup.originPosition + new Vector3(x, 0f, y);
        Instantiate(wallEdgePrefab, pos, Quaternion.Euler(0f, yRotation, 0f), wallsParent);
    }

    private void PlaceWallStraight(float x, float y, float yRotation)
    {
        Vector3 pos = gridSetup.originPosition + new Vector3(x, 0f, y);
        Instantiate(wallStraightPrefab, pos, Quaternion.Euler(0f, yRotation, 0f), wallsParent);
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