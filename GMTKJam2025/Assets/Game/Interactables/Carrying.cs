using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Component for Objects that can Carry Carryables
/// </summary>
public class Carrying : MonoBehaviour
{
    [SerializeField] private float pickUpRange;
    [ReadOnly] private Carryable currentlyCarriedObject;
    [SerializeField]
    private Transform carryTarget;
    [SerializeField]
    private Animator animator;

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
        closestCarryable.transform.SetParent(carryTarget);
        currentlyCarriedObject.DOKill();
        closestCarryable.transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.OutQuad);
        currentlyCarriedObject.GetPickedUp();

        if (animator)
            animator.SetBool("Carrying", true);

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
        currentlyCarriedObject = null;

        if (animator)
            animator.SetBool("Carrying", false);

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
