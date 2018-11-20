using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public State<T> CurrentState;
    public T Owner;

    public StateMachine(T _owner, State<T> _initialState)
    {
        Owner = _owner;
        ChangeState(_initialState);
    }
    
    public void ChangeState(State<T> _newState)
    {
        if (CurrentState != null)
            CurrentState.ExitState(Owner);
        
        CurrentState = _newState;
        _newState.EnterState(Owner);
    }

    public void Update()
    {
        if (CurrentState != null)
            CurrentState.Update(Owner);
    }
}
