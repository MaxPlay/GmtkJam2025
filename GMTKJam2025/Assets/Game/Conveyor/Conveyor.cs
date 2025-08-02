using NaughtyAttributes;
using System;
using UnityEditor;
using UnityEngine;

public abstract class Conveyor : MonoBehaviour
{
    [SerializeField]
    private ConveyorDirection direction;

    [ReadOnly]
    [SerializeField]
    private Conveyor next;

    [ReadOnly]
    [SerializeField]
    private Machine installedMachine;

    public void Tick()
    {

    }

    private void OnDrawGizmos()
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
        DrawArrow.ForGizmo(thisTransform.position + Vector3.up, directionVector);

        Handles.Label(thisTransform.position + Vector3.up * 1.1f, GameManager.CoordinateFromWorld(thisTransform.position).ToString()); 
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
}

public enum ConveyorDirection
{
    Forward,
    Right,
    Back,
    Left
}
