using System;
using UnityEngine;

public abstract class StateBehaviour<T> : MonoBehaviour where T : Enum
{
    [SerializeField] protected T defaultState;
    public T DefaultState => defaultState;

    public abstract T UpdateState(float deltaTime);

    public abstract void EnterState();
    public abstract void ExitState();

    /// <summary>
    /// Invoked if this State is the inítial State for a Machine
    /// </summary>
    public abstract void InitializeState();
}
