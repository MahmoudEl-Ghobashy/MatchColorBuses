using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GridSystem gridSetup;
    public GameObject tilePrefab;
    public GameObject obstaclePrefab;
    public GameObject holePrefab;
    public Transform tileParent;

    private void Start()
    {
        GenerateGrid();
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
}