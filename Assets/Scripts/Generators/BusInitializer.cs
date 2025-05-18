using System;
using System.Collections.Generic;
using UnityEngine;

public class BusInitializer : MonoBehaviour
{
    [SerializeField] private BusSpawner busPrefab;
    [SerializeField] private Transform busParent;
    [SerializeField] private MaterialLibrary materialLibrary;

    public void SpawnBuses(GridSystem setup, PassengerSpawner passengerSpawner)
    {
        BusSpawner bus;
        if (setup.hasBlueBus)
        {
            bus = SpawnBus(setup.blueBusPositions, setup.bluePassagePosition, materialLibrary.Blue, setup);
            passengerSpawner.SpawnPassengersForBus(setup.bluePassagePosition, setup.blueBusPositions.Count, materialLibrary.Blue.main,
                setup, bus.GetComponent<BusController>());
        }
        if (setup.hasRedBus)
        {
            bus = SpawnBus(setup.redBusPositions, setup.redPassagePosition, materialLibrary.Red, setup);
            passengerSpawner.SpawnPassengersForBus(setup.redPassagePosition, setup.redBusPositions.Count, materialLibrary.Red.main,
                setup, bus.GetComponent<BusController>());
        }
        if (setup.hasGreenBus)
        {
            bus = SpawnBus(setup.greenBusPositions, setup.greenPassagePosition, materialLibrary.Green, setup);
            passengerSpawner.SpawnPassengersForBus(setup.greenPassagePosition, setup.greenBusPositions.Count, materialLibrary.Green.main,
                setup, bus.GetComponent<BusController>());

        }
        if (setup.hasOrangeBus)
        {
            bus = SpawnBus(setup.orangeBusPositions, setup.orangePassagePosition, materialLibrary.Orange, setup);
            passengerSpawner.SpawnPassengersForBus(setup.orangePassagePosition, setup.orangeBusPositions.Count, materialLibrary.Orange.main,
                setup, bus.GetComponent<BusController>());
        }
        if (setup.hasPurpleBus)
        {
            bus = SpawnBus(setup.purpleBusPositions, setup.purplePassagePosition, materialLibrary.Purple, setup);
            passengerSpawner.SpawnPassengersForBus(setup.purplePassagePosition, setup.purpleBusPositions.Count, materialLibrary.Purple.main,
                setup, bus.GetComponent<BusController>());
        }
    }

    private BusSpawner SpawnBus(List<int> gridPositions, int passageIndex, BusColorMaterial material, GridSystem setup)
    {
        BusSpawner bus = Instantiate(busPrefab, busParent);
        bus.SetMaterialColors(material.main, material.dull);
        bus.SetBusPositions(CalculateBusPositions(gridPositions, setup));
        for (int i = 0; i < gridPositions.Count; i++)
        {
            GridManager.Instance.AssignGridCell(gridPositions[i]);
        }
        BusController controller = bus.GetComponent<BusController>();
        if (controller)
        {
            controller.SetGridSystem(setup);
            controller.SetPassagePosition(passageIndex);
        }
        return bus;

    }

    private List<Vector3> CalculateBusPositions(List<int> gridPositions, GridSystem setup)
    {
        List<Vector3> worldPositions = new List<Vector3>();

        for (int i = 0; i < gridPositions.Count; i++)
        {
            int gridPos = gridPositions[i];
            int y = gridPos / setup.columns;
            int x = gridPos % setup.columns;
            worldPositions.Add(setup.originPosition + new Vector3(x, 0f, y));
        }

        return worldPositions;
    }
}
