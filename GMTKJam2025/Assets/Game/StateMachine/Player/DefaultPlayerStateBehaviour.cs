using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultPlayerStateBehaviour : StateBehaviour<PlayerStates>
{
    [SerializeField] private PlayerStates interactState = PlayerStates.Interact;
    [SerializeField] private PlayerStates carryState = PlayerStates.Carry;

    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference pickupAction;

    [SerializeField] private Interacting interactionsBehaviour;
    [SerializeField] private Carrying carryingBehaviour;
    [SerializeField] private InputMovementBehaviour movementBehaviour;

    private bool objectInteractingWith;
    private bool objectPickedUp;

    public override PlayerStates UpdateState(float deltaTime)
    {
        if (objectInteractingWith)
        {
            return interactState;
        }
        if (objectPickedUp)
        {
            return carryState;
        }

        movementBehaviour.UpdateMovement(deltaTime);

        return defaultState;
    }

    public override void EnterState()
    {
        RegisterEvents(false);
    }

    private void RegisterEvents(bool unregister)
    {
        if (unregister)
        {
            interactAction.action.started -= TryInteract;
            pickupAction.action.started -= TryPickUp;
        }
        else
        {
            interactAction.action.started += TryInteract;
            pickupAction.action.started += TryPickUp;
        }
    }

    public override void ExitState()
    {
        objectInteractingWith = false;
        objectPickedUp = false;

        RegisterEvents(true);
    }

    public override void InitializeState()
    {
        RegisterEvents(false);
    }

    private void TryInteract(InputAction.CallbackContext obj)
    {
        if (interactionsBehaviour.TryInteract())
        {
            objectInteractingWith = true;
        }
    }

    private void TryPickUp(InputAction.CallbackContext obj)
    {
        if (carryingBehaviour.TryCarry())
        {
            objectPickedUp = true;
        }
    }
}
