using System;
using System.Collections.Generic;
using NaughtyAttributes;
using NUnit.Framework;
using UnityEngine;

public class StateMachineBehaviour<T> : MonoBehaviour where T : Enum
{
#if UNITY_EDITOR
    [ReadOnly] public T currentState;
#endif

    [SerializeField] protected bool updateAfterStateChange;
    [SerializeField] protected T initialState;
    [SerializeField] protected List<StateEntry> states;

    protected StateMachine<T> stateMachine;


    protected void Start()
    {
        stateMachine = new StateMachine<T>(updateAfterStateChange);
        foreach (StateEntry state in states)
        {
            stateMachine.AddState(state.State, state.Behaviour.UpdateState, state.Behaviour.EnterState, state.Behaviour.ExitState, state.Behaviour.InitializeState);
        }
        stateMachine.Initialize(initialState);
    }

    protected void Update()
    {
        stateMachine.Update(Time.deltaTime);
#if UNITY_EDITOR
        currentState = stateMachine.CurrentState;
#endif
    }

    private void OnDestroy()
    {
        stateMachine.Destroy();
    }

    [Button("Collect Attached States")]
    protected void CollectAssignedStates()
    {
        states.Clear();
        foreach (StateBehaviour<T> component in GetComponents<StateBehaviour<T>>())
        {
            states.Add(new StateEntry(component.DefaultState, component));
        }
    }

    [System.Serializable]
    public class StateEntry
    {
        [SerializeField] private T state;
        [SerializeField] private StateBehaviour<T> behaviour;

        public StateEntry(T state, StateBehaviour<T> behaviour)
        {
            this.state = state;
            this.behaviour = behaviour;
        }

        public T State => state;

        public StateBehaviour<T> Behaviour => behaviour;
    }
}
