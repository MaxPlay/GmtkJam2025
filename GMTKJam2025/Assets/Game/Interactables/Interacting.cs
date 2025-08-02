using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component for Objects that can Interact with Interactables
/// </summary>
public class Interacting : MonoBehaviour
{
    [SerializeField] private float interactionRange;

    private void Update()
    {
        //Go Through Interactables and Check to Show Tooltips
    }

    public bool TryInteract(int interactionIndex)
    {
        Collider[] hitInteractables = Physics.OverlapSphere(transform.position, interactionRange, 1 << 7);
        if (hitInteractables.Length == 0)
            return false;
        Interactable closestInteractable = null;
        float closestDistance = -1;
        foreach (Collider collider in hitInteractables)
        {
            if (!collider.TryGetComponent(out Interactable newInteractable) || newInteractable.Blocked)
                continue;
            closestDistance = CheckDistanceAndAssign(closestDistance, newInteractable, ref closestInteractable);
        }

        if (!closestInteractable)
            return false;

        closestInteractable.Interact(interactionIndex);

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
