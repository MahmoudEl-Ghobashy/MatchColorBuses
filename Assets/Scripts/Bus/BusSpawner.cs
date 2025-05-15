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
        Debug.Log("dir " + (positions[1] - positions[0]).normalized * -1);
        busHead.transform.rotation = Quaternion.LookRotation((positions[1] - positions[0]).normalized * -1);
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
        ApplyMaterials(busHead, mainMaterial, dullMaterial);
        ApplyMaterials(busTail, mainMaterial, dullMaterial);
        ApplyMaterials(busSegment, mainMaterial, dullMaterial);
    }

    private void ApplyMaterials(MeshRenderer meshRenderer, Material mainMaterial, Material dullMaterial)
    {
        Material[] materials = meshRenderer.sharedMaterials;

        if (materials.Length < 2)
        {
            Debug.LogWarning($"{meshRenderer.name} does not have enough material slots (expected 2).");
            return;
        }

        materials[0] = mainMaterial;
        materials[1] = dullMaterial;

        meshRenderer.sharedMaterials = materials; // Apply the updated array
    }
}