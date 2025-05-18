using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject wallStraightPrefab;
    [SerializeField] private GameObject wallEdgePrefab;
    [SerializeField] private Transform wallsParent;

    public void PlaceWalls(GridSystem setup)
    {
        int cols = setup.columns;
        int rows = setup.rows;
        Vector3 origin = setup.originPosition;
        //0.6 is spacing from bottom and right side
        //0.4 is spacing from top and left side

        //Set Edges
        PlaceWallEdge(-0.6f, -0.6f, 270,setup);
        PlaceWallEdge(cols - 0.4f, -0.6f, 180,setup);
        PlaceWallEdge(cols - 0.4f, rows - 0.4f, 90,setup);
        PlaceWallEdge(-0.6f, rows - 0.4f, 0,setup);
        //Set walls
        for (int i = 0; i < cols; i++)
        {
            PlaceWallStraight(i, -0.6f, 90,setup);
            PlaceWallStraight(i, rows - 0.4f, 90, setup);
        }

        for (int i = 0; i < rows; i++)
        {
            PlaceWallStraight(-0.6f, i, 0, setup);
            PlaceWallStraight(cols - 0.4f, i, 0, setup);
        }
    }

    private void PlaceWallEdge(float x, float y, float yRotation,GridSystem setup)
    {
        Vector3 pos = setup.originPosition + new Vector3(x, 0f, y);
        Instantiate(wallEdgePrefab, pos, Quaternion.Euler(0f, yRotation, 0f), wallsParent);
    }

    private void PlaceWallStraight(float x, float y, float yRotation,GridSystem setup)
    {
        Vector3 pos = setup.originPosition + new Vector3(x, 0f, y);
        Instantiate(wallStraightPrefab, pos, Quaternion.Euler(0f, yRotation, 0f), wallsParent);
    }

}
