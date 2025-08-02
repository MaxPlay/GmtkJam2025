using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultPlayerStateBehaviour : StateBehaviour<PlayerStates>
{
    [SerializeField] private PlayerStates interactState = PlayerStates.Interact;
    [SerializeField] private PlayerStates carryState = PlayerStates.Carry;

    [SerializeField] InputActionReference interactActionA;
    [SerializeField] InputActionReference interactActionB;
    [SerializeField] InputActionReference interactActionN;
    [SerializeField] InputActionReference pickupAction;

    [SerializeField] private Interacting interactionsBehaviour;
    [SerializeField] private Carrying carryingBehaviour;
    [SerializeField] private InputMovementBehaviour movementBehaviour;

    private bool objectPickedUp;
    private Dictionary<InputAction, int> InteractionActionAssignments = new ();

    private void Awake()
    {
        InteractionActionAssignments[interactActionA.action] = 0;
        InteractionActionAssignments[interactActionB.action] = 1;
        InteractionActionAssignments[interactActionN.action] = 2;
    }

    public override PlayerStates UpdateState(float deltaTime)
    {
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
            interactActionA.action.started -= TryInteract;
            interactActionB.action.started -= TryInteract;
            interactActionN.action.started -= TryInteract;
            pickupAction.action.started -= TryPickUp;
        }
        else
        {
            interactActionA.action.started += TryInteract;
            interactActionB.action.started += TryInteract;
            interactActionN.action.started += TryInteract;
            pickupAction.action.started += TryPickUp;
        }
    }

    public override void ExitState()
    {
        objectPickedUp = false;

        RegisterEvents(true);
    }

    public override void InitializeState()
    {
        RegisterEvents(false);
    }

    private void TryInteract(InputAction.CallbackContext obj)
    {
        interactionsBehaviour.TryInteract(InteractionActionAssignments[obj.action]);
    }

    private void TryPickUp(InputAction.CallbackContext obj)
    {
        if (carryingBehaviour.TryCarry())
        {
            objectPickedUp = true;
        }
    }
}
