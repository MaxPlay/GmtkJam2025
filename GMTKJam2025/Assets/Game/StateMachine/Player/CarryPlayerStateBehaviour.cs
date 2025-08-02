using UnityEngine;
using UnityEngine.InputSystem;

public class CarryPlayerStateBehaviour : StateBehaviour<PlayerStates>
{
    [SerializeField] private PlayerStates leavingState = PlayerStates.Default;
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference pickupAction;

    private bool objectDropped;

    public override PlayerStates UpdateState(float deltaTime)
    {
        if (objectDropped)
            return leavingState;

        return defaultState;
    }

    public override void EnterState()
    {
        RegisterEvents(false);
    }

    public override void ExitState()
    {
        RegisterEvents(true);
        objectDropped = false;
    }

    private void RegisterEvents(bool unregister)
    {
        if (unregister)
        {
            pickupAction.action.started -= TryDropItem;
        }
        else
        {
            pickupAction.action.started += TryDropItem;
        }
    }

    private void TryDropItem(InputAction.CallbackContext obj)
    {
        objectDropped = true;
    }

    public override void InitializeState()
    {
        RegisterEvents(false);
    }
}
