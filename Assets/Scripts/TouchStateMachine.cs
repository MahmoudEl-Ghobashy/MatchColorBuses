using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchStateMachine : MonoBehaviour
{
    private enum TouchState
    {
        Idle,
        Selected,
        Held,
        Released
    }

    private TouchState currentState = TouchState.Idle;
    private Vector2 touchCurrentPos;
    private Vector2 touchStartPos;
    private BusController draggableBus;
    private GameObject hitObject;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
#if UNITY_EDITOR             
        if (Mouse.current.leftButton.isPressed) // Mouse input 
        {
            Vector2 mousePos = Mouse.current.position.value;
            touchStartPos = mousePos;
            TakeAction();
        }
#elif UNITY_ANDROID
        if (Touch.activeTouches.Count > 0) // Touch input
        {
            touchStartPos = Touch.activeTouches[0].screenPosition;
            TakeAction();
        }
#endif
        else
        {
            currentState = TouchState.Released;
            TakeAction();
        }
    }

    private void TakeAction()
    {
        switch (currentState)
        {
            case TouchState.Idle:
                if (isTouchingBusHead(touchStartPos))
                    currentState = TouchState.Selected;
                break;
            case TouchState.Selected:
                if (isTouchingBusHead(touchStartPos))
                    currentState = TouchState.Held;
                break;
            case TouchState.Held:
                touchCurrentPos = touchStartPos;
                if (!draggableBus)
                {
                    if (isTouchingBusHead(touchStartPos))
                    {
                        draggableBus = hitObject.transform.parent.GetComponent<BusController>();
                        draggableBus.BeginDrag();
                    }
                }
                break;
            case TouchState.Released:
                if (draggableBus)
                {
                    draggableBus.StopDrag();
                    draggableBus = null;
                    hitObject = null;
                }

                currentState = TouchState.Idle;
                break;
        }
    }

    private bool isTouchingBusHead(Vector2 touchPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 1.0f);

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("BusHead")))
        {
            Debug.Log("raycast hit");
            hitObject = hit.transform.gameObject;
            return true;
        }

        return false;
    }
}