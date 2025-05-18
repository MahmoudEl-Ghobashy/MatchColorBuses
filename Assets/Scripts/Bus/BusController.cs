using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BusController : MonoBehaviour
{

    [SerializeField] Transform BusHead;
    public List<Transform> BusSegments;
    public Vector3 PassagePosition;
    public List<GameObject> Passengers; 

    private bool canDrag = false;
    private Vector3 worldPos;
    private Tween moveTween;
    private Tween rotateTween;
    private List<Vector3> positionHistory = new List<Vector3>();
    private GridSystem _gridSetup;

    private float moveCooldown = 0.15f;
    private float moveTimer = 0f;
    private Vector3 currentTarget;


    private void Start()
    {
        positionHistory.Add(BusHead.position);
        for (int i = 0; i < BusSegments.Count; i++)
        {
            positionHistory.Add(BusSegments[i].position);
        }
    }

    private void Update()
    {
        if (!canDrag) return;

        moveTimer -= Time.deltaTime;
        if (moveTimer > 0f) return;

        Vector2 screenPos = new Vector2(0, 0);
#if UNITY_EDITOR
        screenPos = Mouse.current.position.ReadValue();
#elif UNITY_ANDROID
        screenPos = Touch.activeTouches[0].screenPosition;
#endif

        Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, Camera.main.transform.position.y);
        Vector3 rawPos = Camera.main.ScreenToWorldPoint(screenPoint);

        Vector3 delta = rawPos - BusHead.position;

        if (delta.magnitude < 0.2f) return;

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

        if (!IsValidGridCell(targetPos) || GridManager.Instance.IsCellOccupied(targetPos)) return;
        if (targetPos == currentTarget) return;

        currentTarget = targetPos;
        moveTimer = moveCooldown;

        // Move head
        MoveTo(BusHead, targetPos);

        // Insert new head position into history
        positionHistory.Insert(0, targetPos);
        GridManager.Instance.AssignGridCell(targetPos);
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
        int maxHistory = BusSegments.Count + 1;
        if (positionHistory.Count > maxHistory)
        {
            GridManager.Instance.FreeGridCell(positionHistory[^1]);
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
       StartCoroutine( CheckForPassagePosition());
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

    public void SetPassagePosition(int passageIndex)
    {
        int x = passageIndex % _gridSetup.columns;
        int y = passageIndex / _gridSetup.columns;

        PassagePosition = _gridSetup.originPosition + new Vector3(x, 0f, y);
    }

    private IEnumerator CheckForPassagePosition()
    {
        bool isTargetReached = false;
        foreach (Vector3 position in positionHistory)
        {
            if (position == PassagePosition)
            {
                //ToDo animate characters 
                yield return StartCoroutine(BeginCharacterAnimation());
                isTargetReached = true;
                break;
            }
        }
        if (isTargetReached)
        {
            foreach (Vector3 position in positionHistory)
            {
                GridManager.Instance.FreeGridCell(position);
            }
           DestroyImmediate(gameObject);
        }
    }

    private IEnumerator BeginCharacterAnimation()
    {
        int passengerIndex = 0;
        
        for (int i = -1; i < BusSegments.Count && passengerIndex < Passengers.Count; i++)
        {
            int passengersToBoard = 4;

            if (i == -1 || i == BusSegments.Count - 1)
                passengersToBoard = 2; // head or tail

            Transform targetSegment;
            if (i ==-1)
                targetSegment = BusHead.transform;
            else
                targetSegment = BusSegments[i];

            for (int j = 0; j < passengersToBoard && passengerIndex < Passengers.Count; j++)
            {
                GameObject passenger = Passengers[passengerIndex];
              

                Vector3 offset = GetOffset(j, passengersToBoard);
                Vector3 targetPos = targetSegment.position + offset;

                passenger.transform.DOMove(targetPos, 0.15f).SetEase(Ease.InOutQuad);
                passengerIndex++;
                passenger.transform.parent = this.transform;
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    private Vector3 GetOffset(int index, int total)
    {
        float spacing = 0.3f;
        float startX = -((total - 1) * spacing) / 2f;
        return new Vector3(startX + index * spacing, 0f, 0f);
    }
}