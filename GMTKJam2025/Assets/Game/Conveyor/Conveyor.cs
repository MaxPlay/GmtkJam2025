using DG.Tweening;
using NaughtyAttributes;
using System;
using UnityEditor;
using UnityEngine;

public abstract class Conveyor : MonoBehaviour
{
    public GameManager GameManager { get; private set; }

    [SerializeField]
    private ConveyorDirection direction;

    [ReadOnly]
    [SerializeField]
    private Conveyor next;

    [SerializeField]
    private Machine installedMachine;

    [SerializeField] private bool allowMachineInstallation = true;

    [SerializeField]
    private ConveyorItem currentItem;
    [ReadOnly]
    [SerializeField]
    private ConveyorItem incomingItem;

    [SerializeField]
    private Vector3 centerPosition;
    [SerializeField]
    private Vector3 outPosition;

    public Vector3 CenterPosition => transform.TransformPoint(centerPosition);
    public Vector3 OutPosition => transform.TransformPoint(outPosition);

    public bool AllowMachineInstallation => allowMachineInstallation && !installedMachine;

    public void Initialize(GameManager gameManager)
    {
        GameManager = gameManager;
        if (currentItem)
            currentItem.SetConveyor(this);
    }

    public void Tick()
    {
        if (installedMachine)
        {
            if (!installedMachine.ModuleTick(currentItem, out ConveyorItem newItem))
            {
                //Machine Broke
            }
            else if(newItem)
            {
                newItem.transform.position = CenterPosition;
                currentItem = newItem;
                currentItem.SetConveyor(this);
            }
        }

        if (next)
        {
            next.PassItem(currentItem);
            currentItem = null;
        }


    }

    private void PassItem(ConveyorItem item)
    {
        incomingItem = item;
    }

    private void OnDrawGizmos()
    {
        Transform thisTransform = transform;
        Vector3 thisPosition = thisTransform.position;
        Vector3 directionVector = direction switch
        {
            ConveyorDirection.Forward => thisTransform.forward,
            ConveyorDirection.Right => thisTransform.right,
            ConveyorDirection.Back => -thisTransform.forward,
            ConveyorDirection.Left => -thisTransform.right,
            _ => throw new Exception("Invalid direction"),
        };
        DrawArrow.ForGizmo(thisPosition + Vector3.up, directionVector);

        Handles.Label(thisPosition + Vector3.up * 1.1f, GameManager.CoordinateFromWorld(thisPosition).ToString());

        Gizmos.color = Color.white;
        Gizmos.DrawCube(thisTransform.TransformPoint(centerPosition), Vector3.one * 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(thisTransform.TransformPoint(outPosition), Vector3.one * 0.2f);
    }

    public void InstallMachine(Machine machine)
    {
        if (allowMachineInstallation)
            installedMachine = machine;
    }

    public Vector2Int GetDeliveryCoordinate()
    {
        Transform thisTransform = transform;
        Vector3 directionVector = direction switch
        {
            ConveyorDirection.Forward => thisTransform.forward,
            ConveyorDirection.Right => thisTransform.right,
            ConveyorDirection.Back => -thisTransform.forward,
            ConveyorDirection.Left => -thisTransform.right,
            _ => throw new Exception("Invalid direction"),
        };
        return GameManager.CoordinateFromWorld(thisTransform.position + directionVector * 2);
    }

    public void SetNext(Conveyor other)
    {
        next = other;
    }

    public void Receive()
    {
        if (incomingItem)
        {
            currentItem = incomingItem;
            incomingItem = null;

            Conveyor other = currentItem.Conveyor;
            Sequence moveSequence = DOTween.Sequence();
            other.MoveToOut(currentItem.transform, moveSequence);
            currentItem.SetConveyor(this);
            MoveIn(currentItem.transform, moveSequence, NewConveyorReached);
        }
    }

    private void NewConveyorReached()
    {
        if (installedMachine)
        {
            //TODO(bz): Machine can break here
            ConveyorItem newItem = installedMachine.ApplyToItem(currentItem);
            if (newItem != currentItem)
            {
                Destroy(currentItem.gameObject);
                currentItem = newItem;
            }
            //Replace current Item with new Item.
        }
    }

    protected abstract void MoveToOut(Transform item, Sequence moveSequence);
    protected abstract void MoveIn(Transform item, Sequence moveSequence, TweenCallback reachedCallback);
}

public enum ConveyorDirection
{
    Forward,
    Right,
    Back,
    Left
}
