using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class BusController : MonoBehaviour
{

    [SerializeField] Transform BusHead;
    public List<Transform> BusSegments;
    [SerializeField] float segmentSpacing = 1.2f;
    [SerializeField] float followSmoothness = 0.15f;

    private bool canDrag = false;
    private Vector3 worldPos;
    private Tween moveTween;
    private Tween rotateTween;
    private List<Vector3> positionHistory = new List<Vector3>();
    private GridSystem _gridSetup;

    private float moveCooldown = 0.2f;
    private float moveTimer = 0f;
    private Vector3 currentTarget;


    private void Start()
    {
        positionHistory.Add(BusHead.position);
    }

    private void Update()
    {
        if (!canDrag) return;

        moveTimer -= Time.deltaTime;
        if (moveTimer > 0f) return;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, Camera.main.transform.position.y);
        Vector3 rawPos = Camera.main.ScreenToWorldPoint(screenPoint);

        Vector3 delta = rawPos - BusHead.position;

        if (delta.magnitude < 0.2f) return; // ignore tiny drags

        Vector3 direction;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.z))
            direction = new Vector3(Mathf.Sign(delta.x), 0f, 0f); // Left or right
        else
            direction = new Vector3(0f, 0f, Mathf.Sign(delta.z)); // Up or down

        Vector3 targetPos = BusHead.position + direction;

        targetPos = new Vector3(
            Mathf.Round(targetPos.x),
            0f,
            Mathf.Round(targetPos.z)
        );

        if (!IsValidGridCell(targetPos) || IsOccupiedByBus(targetPos)) return;
        if (targetPos == currentTarget) return;

        currentTarget = targetPos;
        moveTimer = moveCooldown;

        // Move head
        MoveTo(BusHead, targetPos);

        // Insert new head position into history
        positionHistory.Insert(0, targetPos);

        // Move segments
        for (int i = 0; i < BusSegments.Count; i++)
        {
            int index = (i + 1);
            if (positionHistory.Count > index)
            {
                Vector3 segmentTarget = positionHistory[index];
                MoveTo(BusSegments[i], segmentTarget);
            }
        }

        // Clean up old positions
        int maxHistory = BusSegments.Count + 2;
        if (positionHistory.Count > maxHistory)
        {
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
    }

    private void MoveTo(Transform t, Vector3 target)
    {
        Vector3 dir = target - t.position;
        if (dir.magnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            t.DORotateQuaternion(rot, moveCooldown / 2f).SetEase(Ease.Linear);
        }

        t.DOMove(target, moveCooldown).SetEase(Ease.Linear);
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
    internal void SetGridSystem(GridSystem gridSetup)
    {
        _gridSetup = gridSetup;
    }

    private bool IsValidGridCell(Vector3 worldPosition)
    {
        Vector3 local = worldPosition - _gridSetup.originPosition;
        int x = Mathf.RoundToInt(local.x);
        int y = Mathf.RoundToInt(local.z);

        int index = y * _gridSetup.columns + x;
        bool inBounds = x >= 0 && x < _gridSetup.columns && y >= 0 && y < _gridSetup.rows;
        bool notBlocked = !_gridSetup.holeIndex.Contains(index) && !_gridSetup.obstacleIndex.Contains(index);

        return inBounds && notBlocked;
    }

    private bool IsOccupiedByBus(Vector3 worldPosition)
    {
        foreach (Transform segment in BusSegments)
        {
            if (Vector3.Distance(segment.position, worldPosition) < 0.1f)
                return true;
        }
        return false;
    }
}