using System;
using System.Collections.Generic;
using UnityEngine;

public class BusSpawner : MonoBehaviour
{
    public BusController BusController;

    public MeshRenderer busHead;
    public MeshRenderer busSegment;
    public MeshRenderer busTail;

    private Vector3 _lastSpawnedPosition;


    private void Start()
    {
        BusController = GetComponent<BusController>();
    }

    public void SetBusPositions(List<Vector3> positions)
    {
        busHead.transform.position = positions[0];
        _lastSpawnedPosition = busHead.transform.position;

        for (int i = 1; i < positions.Count - 1; i++)
        {
            GameObject segment = Instantiate(busSegment.gameObject, positions[i], Quaternion.identity, this.transform);
            segment.SetActive(true);
            BusController.BusSegments.Add(segment.transform);
            segment.transform.LookAt(_lastSpawnedPosition);
            _lastSpawnedPosition = positions[i];
        }

        busTail.transform.position = positions[^1];
        BusController.BusSegments.Add(busTail.transform);
        busTail.transform.LookAt(_lastSpawnedPosition);
    }

    public void SetMaterialColors(Material mainMaterial, Material dullMaterial)
    {
        busHead.materials[0] = mainMaterial;
        busSegment.materials[0] = mainMaterial;
        busTail.materials[0] = mainMaterial;

        busHead.materials[1] = dullMaterial;
        busSegment.materials[1] = dullMaterial;
        busTail.materials[1] = dullMaterial;
    }
}