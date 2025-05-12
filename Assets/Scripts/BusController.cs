using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class BusController : MonoBehaviour
{
    public Transform BusHead;
    public List<Transform> BusSegments;
    public float segmentSpacing = 1.2f;
    public float followSmoothness = 0.15f;

    private bool canDrag = false;
    private Vector3 worldPos;
    private Tween moveTween;
    private Tween rotateTween;
    private List<Vector3> positionHistory = new List<Vector3>();

    private void Start()
    {
        positionHistory.Add(BusHead.position);
    }

    private void Update()
    {
        if (canDrag)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, Camera.main.transform.position.y);
            worldPos = Camera.main.ScreenToWorldPoint(screenPoint);


            positionHistory.Insert(0, BusHead.position);

            // Limit position history size
            int maxHistory = BusSegments.Count * Mathf.RoundToInt(segmentSpacing / Time.deltaTime);
            if (positionHistory.Count > maxHistory)
            {
                positionHistory.RemoveAt(positionHistory.Count - 1);
            }

            //Calculate direction to detect rotation
            Vector3 direction = worldPos - BusHead.position;

            //Only rotate when value is higher than a threshold to avoid rotating all the time like a crazy head
            if (direction.magnitude > 0.05f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                rotateTween?.Kill();
                rotateTween = BusHead.DORotateQuaternion(targetRotation, 0.1f).SetEase(Ease.Linear);
            }

            moveTween?.Kill();
            moveTween = BusHead.DOMove(worldPos, 0.1f).SetEase(Ease.Linear);

            for (int i = 0; i < BusSegments.Count; i++)
            {
                int index = Mathf.Clamp((i + 1) * Mathf.RoundToInt(segmentSpacing / Time.deltaTime), 0,
                    positionHistory.Count - 1);
                Vector3 targetPos = positionHistory[index];

                //Calculate direction to detect rotation
                direction = targetPos - BusSegments[i].position;

                //Only rotate when value is higher than a threshold to avoid rotating all the time like a crazy head
                if (direction.magnitude > 0.05f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    BusSegments[i]?.DOKill();
                    BusSegments[i].DORotateQuaternion(targetRotation, 0.1f).SetEase(Ease.Linear);
                }

                BusSegments[i]?.DOKill();
                BusSegments[i].DOMove(targetPos, followSmoothness).SetEase(Ease.Linear);
            }
        }
    }

    public void BeginDrag()
    {
        canDrag = true;
    }

    public void StopDrag()
    {
        canDrag = false;
        moveTween?.Kill();
        rotateTween?.Kill();
    }
}