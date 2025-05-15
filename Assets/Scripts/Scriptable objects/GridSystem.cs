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
    public int bluePassagePosition;

    public bool hasRedBus;
    public List<int> redBusPositions;
    public int redPassagePosition;

    public bool hasGreenBus;
    public List<int> greenBusPositions;
    public int greenPassagePosition;

    public bool hasOrangeBus;
    public List<int> orangeBusPositions;
    public int orangePassagePosition;

    public bool hasPurpleBus;
    public List<int> purpleBusPositions;
    public int purplePassagePosition;

    private void OnValidate()
    {
        originPosition = new Vector3(-columns / 2, 0, -rows / 2);

        //Todo Validate Bus positions list to be connected
        //Todo Validate passages to be on edge cells 
    }
}