public abstract class State<T>
{
    public abstract void EnterState(T owner);
    public abstract void UpdateStateFromServer(float value);
    public abstract void UpdateState(T owner);
}
