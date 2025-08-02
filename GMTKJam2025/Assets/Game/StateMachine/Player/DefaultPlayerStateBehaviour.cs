using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultPlayerStateBehaviour : StateBehaviour<PlayerStates>
{
    [SerializeField] private PlayerStates interactState = PlayerStates.Interact;
    [SerializeField] private PlayerStates carryState = PlayerStates.Carry;

    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference pickupAction;

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
        transform.Translate(new Vector3(moveAction.action.ReadValue<Vector2>().x, 0, moveAction.action.ReadValue<Vector2>().y) * Time.deltaTime);

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
        objectInteractingWith = true;
    }

    private void TryPickUp(InputAction.CallbackContext obj)
    {
        objectPickedUp = true;
    }
}
