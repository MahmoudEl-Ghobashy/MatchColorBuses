using UnityEngine;
using UnityEngine.InputSystem;

public class BusController : MonoBehaviour
{
    private bool canDrag = false;
    private Vector3 worldPos;

    private void Update()
    {
        if (canDrag)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, Camera.main.transform.position.y);
            worldPos = Camera.main.ScreenToWorldPoint(screenPoint);

            transform.position = worldPos;
        }
    }

    public void BeginDrag()
    {
        canDrag = true;
        Debug.Log("begin drag");
    }

    public void StopDrag()
    {
        canDrag = false;
        Debug.Log("stop drag");
    }
}