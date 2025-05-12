using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GridSystem gridSetup;
    public GameObject tilePrefab;
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
                Vector3 worldPos = gridSetup.originPosition +
                                   new Vector3(x, 0f, y);

                GameObject tile = Instantiate(tilePrefab, worldPos, tilePrefab.transform.rotation, tileParent);
                tile.name = $"grid{x}_{y}";
            }
        }
    }
}