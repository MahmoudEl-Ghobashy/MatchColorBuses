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
        ValidateData();
    }

    public void ValidateData()
    {
        originPosition = new Vector3(-columns / 2, 0, -rows / 2);
    }

    public void ResetAllBuses()
    {
        for (int busCount = 0; busCount < 5; busCount++)
        {
            switch (busCount)
            {
                case 0: hasBlueBus = false; break;
                case 1: hasGreenBus = false; break;
                case 2: hasPurpleBus = false; break;
                case 3: hasOrangeBus = false; break;
                case 4: hasRedBus = false; break;
            }
        }
    }

    public void AddObstacle(int index)
    {
        if (obstacleIndex.Contains(index))
            obstacleIndex.Remove(index);
        else
            obstacleIndex.Add(index);
    }

    public void AddBus(List<int> busPositions, int busColor)
    {
        switch (busColor)
        {
            case 0:         //Blue
                hasBlueBus = true;
                blueBusPositions = busPositions;
                break;
            case 1:
                hasGreenBus = true;
                greenBusPositions = busPositions;
                break;
            case 2:
                hasPurpleBus = true;
                purpleBusPositions = busPositions;
                break;
            case 3:
                hasOrangeBus = true;
                orangeBusPositions = busPositions;
                break;
            case 4:
                hasRedBus = true;
                redBusPositions = busPositions;
                break;
        }
    }

    public void SetPassagePosition(int passagePosition, int passageColor)
    {
        switch (passageColor)
        {
            case 0:
                bluePassagePosition = passagePosition; break;
            case 1:
                greenPassagePosition = passagePosition; break;
            case 2:
                purplePassagePosition = passagePosition; break;
            case 3:
                orangePassagePosition = passagePosition; break;
            case 4:
                redPassagePosition = passagePosition; break;
        }
    }
}