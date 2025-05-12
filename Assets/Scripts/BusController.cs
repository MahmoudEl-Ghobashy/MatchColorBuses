using UnityEngine;
using UnityEngine.InputSystem;

public class BusController : MonoBehaviour
{
    private bool canDrag = false;

    private void Update()
    {
        if (canDrag)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            transform.position = Camera.main.ScreenToWorldPoint(screenPos);
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