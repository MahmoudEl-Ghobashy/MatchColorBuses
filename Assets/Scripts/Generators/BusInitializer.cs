using System.Collections.Generic;
using UnityEngine;

public class BusInitializer : MonoBehaviour
{
    [SerializeField] private BusSpawner busPrefab;
    [SerializeField] private Transform busParent;
    [SerializeField] private MaterialLibrary materialLibrary;

    public void SpawnBuses(GridSystem setup, PassengerSpawner passengerSpawner)
    {
        if (setup.hasBlueBus)
        {
            SpawnBus(setup.blueBusPositions, setup.bluePassagePosition, materialLibrary.Blue, setup);
            passengerSpawner.SpawnPassengersForBus(setup.bluePassagePosition, setup.blueBusPositions.Count, materialLibrary.Blue.main, setup);
        }
        if (setup.hasRedBus)
        {
            SpawnBus(setup.redBusPositions, setup.redPassagePosition, materialLibrary.Red, setup);
            passengerSpawner.SpawnPassengersForBus(setup.redPassagePosition, setup.redBusPositions.Count, materialLibrary.Red.main, setup);
        }
        if (setup.hasGreenBus)
        {
            SpawnBus(setup.greenBusPositions, setup.greenPassagePosition, materialLibrary.Green, setup);
            passengerSpawner.SpawnPassengersForBus(setup.greenPassagePosition, setup.greenBusPositions.Count, materialLibrary.Green.main, setup);

        }
        if (setup.hasOrangeBus)
        {
            SpawnBus(setup.orangeBusPositions, setup.orangePassagePosition, materialLibrary.Orange, setup);
            passengerSpawner.SpawnPassengersForBus(setup.orangePassagePosition, setup.orangeBusPositions.Count, materialLibrary.Orange.main, setup);
        }
        if (setup.hasPurpleBus)
        {
            SpawnBus(setup.purpleBusPositions, setup.purplePassagePosition, materialLibrary.Purple, setup);
            passengerSpawner.SpawnPassengersForBus(setup.purplePassagePosition, setup.purpleBusPositions.Count, materialLibrary.Purple.main, setup);
        }
    }

    private void SpawnBus(List<int> gridPositions, int passageIndex, BusColorMaterial material, GridSystem setup)
    {
        BusSpawner bus = Instantiate(busPrefab, busParent);
        bus.SetMaterialColors(material.main, material.dull);
        bus.SetBusPositions(CalculateBusPositions(gridPositions, setup));
        if (bus.GetComponent<BusController>())
            bus.GetComponent<BusController>().SetGridSystem(setup);

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
