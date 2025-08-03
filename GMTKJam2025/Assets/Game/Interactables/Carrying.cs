using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Component for Objects that can Carry Carryables
/// </summary>
public class Carrying : MonoBehaviour
{
    [SerializeField] private float pickUpRange;
    [ReadOnly] private Carryable currentlyCarriedObject;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);
    }

    public bool TryCarry()
    {
        Collider[] hitCarryables = Physics.OverlapSphere(transform.position, pickUpRange, 1 << 7);
        if (hitCarryables.Length == 0)
            return false;
        Carryable closestCarryable = null;
        float closestDistance = -1;
        foreach (Collider collider in hitCarryables)
        {
            if(!collider.TryGetComponent(out Carryable newCarryable) || newCarryable.Blocked)
                continue;
            closestDistance = CheckDistanceAndAssign(closestDistance, newCarryable, ref closestCarryable);
        }

        if (!closestCarryable)
            return false;

        currentlyCarriedObject = closestCarryable;
        closestCarryable.transform.SetParent(transform);
        closestCarryable.transform.position = transform.position;
        currentlyCarriedObject.GetPickedUp();

        return true;
    }

    public bool TryDropItem()
    {
        Collider[] hitCarryables = Physics.OverlapSphere(transform.position, pickUpRange, 1 << 8);
        if (hitCarryables.Length == 0)
            return false;
        Conveyor closestConveyor = null;
        float closestDistance = -1;
        foreach (Collider collider in hitCarryables)
        {
            if (!collider.TryGetComponent(out Conveyor newConveyor) || !newConveyor.AllowMachineInstallation)
                continue;
            closestDistance = CheckDistanceAndAssign(closestDistance, newConveyor, ref closestConveyor);
        }

        if (!closestConveyor)
            return false;

        currentlyCarriedObject.GetDroppedOff(closestConveyor);
        currentlyCarriedObject.transform.position = closestConveyor.transform.position;
        currentlyCarriedObject.transform.SetParent(closestConveyor.transform);
        currentlyCarriedObject = null;

        return true;
    }

    private float CheckDistanceAndAssign<T>(float closestDistance, T newConveyor, ref T closestConveyor) where T : Component
    {
        if (closestDistance < 0)
        {
            closestConveyor = newConveyor;
            closestDistance = Vector3.SqrMagnitude(closestConveyor.transform.position - transform.position);
            return closestDistance;
        }
        float distance = Vector3.SqrMagnitude(newConveyor.transform.position - transform.position);
        if (distance < closestDistance)
        {
            closestConveyor = newConveyor;
            closestDistance = distance;
        }

        return closestDistance;
    }
}
