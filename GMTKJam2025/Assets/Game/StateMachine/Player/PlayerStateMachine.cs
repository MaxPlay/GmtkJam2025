using UnityEngine;

public enum PlayerStates
{
    Default,
    Interact,
    Carry
}

public class PlayerStateMachine : StateMachineBehaviour<PlayerStates>
{

}
