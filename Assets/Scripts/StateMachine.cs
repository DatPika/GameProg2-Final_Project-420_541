using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    private IState currentState;

    public StateType GetCurrentStateType()
    {
        return currentState.Type;
    }

    public void AddState(IState state)
    {
        if (!states.ContainsKey(state.Type))
        {
            states.Add(state.Type, state);
        }
    }

    public void TransitionToState(StateType type)
    {
        currentState?.Exit();
        currentState = states[type];
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Execute();
    }
}
