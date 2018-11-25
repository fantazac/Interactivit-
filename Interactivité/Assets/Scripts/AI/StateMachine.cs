public class StateMachine<T>
{
    public State<T> CurrentState;
    public T Owner;

    public StateMachine(T owner)
    {
        Owner = owner;
    }

    public void ChangeState(State<T> newState)
    {
        CurrentState = newState;
        newState.EnterState(Owner);
        newState.InitState();
    }

    public void UpdateStateMachine()
    {
        if (CurrentState != null)
        {
            CurrentState.UpdateState(Owner);
        }
    }
}
