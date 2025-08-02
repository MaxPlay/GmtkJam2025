using System;
using UnityEngine;

public abstract class StateBehaviour<T> : MonoBehaviour where T : Enum
{
    public abstract T UpdateState(float deltaTime);

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void InitializeState();
}
