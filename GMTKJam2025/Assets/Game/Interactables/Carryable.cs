using UnityEngine;
using UnityEngine.Events;

public class Carryable : MonoBehaviour
{
    [SerializeField]
    private bool blocked;

    [SerializeField] private UnityEvent onPickUp;
    [SerializeField] private UnityEvent<Conveyor> onDropOff;

    public bool Blocked => blocked;

    public UnityEvent OnPickUp => onPickUp;
    public UnityEvent<Conveyor> OnDropOff => onDropOff;

    public void SetBlocked(bool blocked)
    {
        this.blocked = blocked;
    }

    public void GetPickedUp()
    {
        OnPickUp.Invoke();
    }

    public void GetDroppedOff(Conveyor newConveyor)
    {
        OnDropOff.Invoke(newConveyor);
    }
}
