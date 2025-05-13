using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridSystem", menuName = "Scriptable Objects/GridSystem")]
public class GridSystem : ScriptableObject
{
    [Header("Grid Dimensions")] public int columns = 10; // X axis
    public int rows = 10; // Z axis
    public Vector3 originPosition = Vector3.zero;

    [Header("Different cell types")] public List<int> obstacleIndex;
    public List<int> holeIndex;

    [Header("Buses on grid")] public bool hasBlueBus;
    public List<int> blueBusPositions;

    public bool hasRedBus;
    public List<int> redBusPositions;

    public bool hasGreenBus;
    public List<int> greenBusPositions;

    public bool hasOrangeBus;
    public List<int> orangeBusPositions;

    public bool hasPurpleBus;
    public List<int> purpleBusPositions;

    private void OnValidate()
    {
        originPosition = new Vector3(-columns / 2, 0, -rows / 2);
    }
}