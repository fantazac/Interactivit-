using UnityEngine;

public abstract class State<T>
{
    public abstract void EnterState(T owner);
    public abstract void InitState();
    public abstract void StartState();
    public abstract void UpdateStateFromServer(float value);
    public abstract void UpdateStateFromServerWithVector(float value1, Vector3 value2);
    public abstract void UpdateState(T owner);
}
