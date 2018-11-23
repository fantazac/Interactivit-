public class StateMachine<T>
{
    public State<T> CurrentState;
    public T Owner;

    public StateMachine(T owner, State<T> initialState)
    {
        Owner = owner;
        ChangeState(initialState);
    }

    public void ChangeState(State<T> newState)
    {
        CurrentState = newState;
        newState.EnterState(Owner);
    }

    public void UpdateStateMachine()
    {
        if (CurrentState != null)
        {
            CurrentState.UpdateState(Owner);
        }
    }
}
