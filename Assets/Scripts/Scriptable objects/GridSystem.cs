using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GridSystem", menuName = "Scriptable Objects/GridSystem")]
public class GridSystem : ScriptableObject
{
    public int columns = 10; // X axis
    public int rows = 10; // Z axis

    public Vector3 originPosition = Vector3.zero;

    private void OnValidate()
    {
        originPosition = new Vector3(-columns / 2, 0, -rows / 2);
    }
}