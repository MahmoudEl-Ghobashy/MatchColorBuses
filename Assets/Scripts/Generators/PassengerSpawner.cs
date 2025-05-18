using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject passengerPrefab;
    [SerializeField] private GameObject passengerGate;
    [SerializeField] private Transform wallsParent;

    public void SpawnPassengersForBus(int passagePosition, int busLength, Material material,GridSystem setup)
    {
        Vector3 direction;
        Vector3 passagePos;
        Quaternion gateRotation;

        int y = passagePosition / setup.columns;
        int x = passagePosition % setup.columns;

        if (y == 0)
        {
            passagePos = setup.originPosition + new Vector3(x, 0, y - 0.6f);
            gateRotation = Quaternion.Euler(0, 0, 0);
            direction = Vector3.back;
        }
        else if (y == setup.rows - 1)
        {
            passagePos = setup.originPosition + new Vector3(x, 0, y + 0.6f);
            gateRotation = Quaternion.Euler(0, 180, 0);
            direction = Vector3.forward;
        }
        else if (x == 0)
        {
            passagePos = setup.originPosition + new Vector3(x - 0.6f, 0, y);
            gateRotation = Quaternion.Euler(0, 90, 0);
            direction = Vector3.left;
        }
        else if (x == setup.columns - 1)
        {
            passagePos = setup.originPosition + new Vector3(x + 0.6f, 0, y);
            gateRotation = Quaternion.Euler(0, 270, 0);
            direction = Vector3.right;
        }
        else
        {
            return;
        }

        GameObject passage = Instantiate(passengerGate, passagePos, gateRotation, wallsParent);
        var mr = passage.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            var materials = mr.sharedMaterials;
            materials[0] = material;
            mr.sharedMaterials = materials;
        }

        SpawnPassengerLine(passagePos, direction, busLength * 4, material);
    }

    private void SpawnPassengerLine(Vector3 startPos, Vector3 direction, int count, Material material)
    {
        float forwardSpacing = 0.4f;
        float sideOffset = 0.15f;

        Vector3 perpendicular = Vector3.Cross(Vector3.up, direction).normalized;

        for (int i = 1; i <= count; i++)
        {
            Vector3 forwardOffset = direction * (i * forwardSpacing);
            float sideDir = (i % 2 == 0) ? 1f : -1f;
            Vector3 side = perpendicular * sideOffset * sideDir;

            Vector3 finalPosition = startPos + forwardOffset + side;
            GameObject passenger = Instantiate(passengerPrefab, finalPosition, Quaternion.identity, wallsParent);

            if (direction == Vector3.left || direction == Vector3.right)
            {
                passenger.transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            var meshRenderer = passenger.GetComponentInChildren<SkinnedMeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.sharedMaterial = material;
            }
        }
    }
}
