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
        //TODO(bz): Get this from a cached List
        var type = FindObjectsOfType<Carryable>();
        foreach (Carryable item in type)
        {
            if (!(Vector3.Distance(item.transform.position, this.transform.position) < pickUpRange)) 
                continue;

            currentlyCarriedObject = item;
            item.transform.SetParent(transform);
            item.transform.position = transform.position;
            currentlyCarriedObject.GetPickedUp();
            return true;
        }

        return false;
    }

    public bool TryDropItem()
    {
        currentlyCarriedObject.GetDroppedOff();
        currentlyCarriedObject.transform.SetParent(null);
        currentlyCarriedObject = null;

        return true;
    }
}
