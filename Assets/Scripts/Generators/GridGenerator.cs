using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab, holePrefab, obstaclePrefab;
    [SerializeField] private Transform tileParent;

    private Dictionary<int, GameObject> gridCells = new();

    public void Generate(GridSystem setup)
    {
        GameObject go;
        gridCells.Clear();

        for (int y = 0; y < setup.rows; y++)
        {
            for (int x = 0; x < setup.columns; x++)
            {
                int index = y * setup.columns + x;
                Vector3 worldPos = setup.originPosition + new Vector3(x, 0f, y);

                if (setup.holeIndex.Contains(index))
                     go =  Instantiate(holePrefab, tileParent);
                else if (setup.obstacleIndex.Contains(index))
                   go=  Instantiate(obstaclePrefab, tileParent);
                else
                    go = Instantiate(tilePrefab ,tileParent);

                go.transform.position = worldPos;
                GridManager.Instance.StoreGridCellToIndex(index, go);
            }
        }
    }
}
    

