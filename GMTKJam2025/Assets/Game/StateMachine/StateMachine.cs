using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : Enum
{
    private readonly Dictionary<T, State> states = new();
    private T currentState;
    private bool updateAfterStateChange;

    public StateMachine(bool updateAfterStateChange)
    {
        this.updateAfterStateChange = updateAfterStateChange;
    }

    public T CurrentState
    {
        get => currentState;
        private set
        {
            currentState = value;
            TestCurrentState();
        }
    }

    public void Initialize(T startState)
    {
        CurrentState = startState;
        states[CurrentState].InitFunction?.Invoke();
    }

    public void Update(float deltaTime)
    {
        if (!states.ContainsKey(CurrentState))
            return;
        T newState = states[CurrentState].UpdateFunction.Invoke(deltaTime);
        if (!newState.Equals(CurrentState))
        {
            states[CurrentState].ExitFunction?.Invoke();
            CurrentState = newState;
            if (states.ContainsKey(CurrentState))
                states[CurrentState].EnterFunction?.Invoke();

            if (updateAfterStateChange && states.ContainsKey(CurrentState))
                states[CurrentState].UpdateFunction.Invoke(deltaTime);
        }
    }

    public void Destroy()
    {
        if (!states.ContainsKey(CurrentState))
            return;
        states[CurrentState].ExitFunction?.Invoke();
    }

    private void TestCurrentState()
    {
        if (!states.ContainsKey(CurrentState))
            throw (new NotImplementedException(
                $"State {CurrentState} has not been implement for StateMachine of Type {typeof(T)}"));
    }

    public StateMachine<T> AddState(T stateType, Func<float, T> updateFunction, Action enterFunction = null, Action exitFunction = null, Action initFunction = null)
    {
        if (states.ContainsKey(stateType))
        {
            Debug.LogError($"State {stateType} already implemented");
            return this;
        }

        states.Add(stateType, new State()
        {
            UpdateFunction = updateFunction,
            EnterFunction = enterFunction,
            ExitFunction = exitFunction,
            InitFunction = initFunction
        });

        return this;
    }

    private class State
    {
        public Func<float, T> UpdateFunction;
        public Action EnterFunction;
        public Action ExitFunction;
        /// <summary>
        /// Invoked if this State is the inítial State for a Machine
        /// </summary>
        public Action InitFunction;
    }
}
