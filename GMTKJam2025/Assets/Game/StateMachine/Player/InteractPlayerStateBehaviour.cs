using UnityEngine;
using UnityEngine.InputSystem;

public class InteractPlayerStateBehaviour : StateBehaviour<PlayerStates>
{
    [SerializeField] private PlayerStates leavingState = PlayerStates.Default;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] private Interacting interactionsBehaviour;

    public override PlayerStates UpdateState(float deltaTime)
    {
        if (!interactAction.action.IsPressed())
            return leavingState;

        return defaultState;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void InitializeState()
    {
    }
}
