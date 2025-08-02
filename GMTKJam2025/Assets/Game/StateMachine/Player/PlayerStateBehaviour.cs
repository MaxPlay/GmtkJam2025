using UnityEngine;

public class PlayerStateBehaviour : StateBehaviour<PlayerStates>
{
    public override PlayerStates UpdateState(float deltaTime)
    {
        return PlayerStates.Default;
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
