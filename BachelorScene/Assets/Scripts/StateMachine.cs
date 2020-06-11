using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public State<T> CurrentState { get; protected set; }
    public State<T> PreviousState { get; protected set; }

    public List<StateChangeRecord<T>> StateChanges { get; protected set; }

    public Dictionary<T, State<T>> States { get; protected set; }

    public Func<long> TimingFunction { get; set; } = () => DateTimeOffset.Now.ToUnixTimeMilliseconds();

    public bool Started { get; protected set; } = false;
    public T InitialState { get; protected set; }

    public bool Locked { get; set; } = false;
    public bool TrackChanges { get; set; } = true;

    public static Dictionary<U, State<U>> GetStateDict<U>(List<State<U>> states) {
        Dictionary<U, State<U>> dictStates = new Dictionary<U, State<U>>();

        foreach (State<U> state in states)
        {
            dictStates.Add(state.Name, state);
        }

        return dictStates;
    }

    public static Dictionary<U, State<U>> GetStateDict<U>(Dictionary<U, State<U>> states)
    {
        return states;
    }

    public static U GetStateName<U>(State<U> state)
    {
        return state.Name;
    }

    public static U GetStateName<U>(U state)
    {
        return state;
    }

    public StateMachine(List<State<T>> states, State<T> currentState = default) : this(GetStateDict(states), GetStateName(currentState)) { }
    public StateMachine(List<State<T>> states, T currentState = default) : this(GetStateDict(states), GetStateName(currentState)) { }
    public StateMachine(Dictionary<T, State<T>> states, State<T> currentState = default) : this(GetStateDict(states), GetStateName(currentState)) { }

    public StateMachine(Dictionary<T, State<T>> states, T currentState = default)
    {
        States = states;
        StateChanges = new List<StateChangeRecord<T>>();

        InitialState = currentState;
    }

    // Calls the curren states OnStay method
    // Stays in same state if it returns null, otherwise goes to the state with the return values name
    public void Update()
    {
        if (!Started)
        {
            SetState(InitialState, true);

            Started = true;
        }

        if (CurrentState != null && CurrentState.OnStay != null)
        {
            T newStateKey = CurrentState.OnStay();

            if (newStateKey != null && !Locked)
            {
                State<T> newState;

                if (States.TryGetValue(newStateKey, out newState))
                {
                    SetState(newState);
                }
            }
        }
    }

    public void TrackStateChange(State<T> oldState, State<T> newState)
    {
        if (TrackChanges)
        {
            if (oldState != null)
            {
                StateChangeRecord<T> record = new StateChangeRecord<T>(oldState.Name, newState.Name, TimingFunction.Invoke());

                StateChanges.Add(record);
            }
        }
    }

    // Calls previous state OnLeave if possible
    // Calls new state OnEnter if possible
    public bool SetState(State<T> newState, bool force = false)
    {
        if (!Locked || force)
        {
            CurrentState?.OnLeave?.Invoke();

            PreviousState = CurrentState;
            CurrentState = newState;

            TrackStateChange(PreviousState, CurrentState);

            CurrentState.OnEnter?.Invoke();

            return true;
        }

        return false;
    }

    public bool SetState(T newStateKey, bool force = false)
    {
        State<T> newState;

        if (States.TryGetValue(newStateKey, out newState))
        {
            return SetState(newState, force);
        }

        return false;
    }

    public T AddState(State<T> state)
    {
        try
        {
            States.Add(state.Name, state);

            return state.Name;
        }
        catch (ArgumentException)
        {
            return default;
        }
    }

    // Doesn't clean up the state
    public void RemoveState(State<T> state)
    {
        States.Remove(state.Name);
    }

    // Doesn't clean up the state
    public void RemoveState(T stateKey)
    {
        States.Remove(stateKey);
    }
}
