using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    public abstract void EnterState(T _owner);
    public abstract void ExitState(T _owner);
    public abstract void Update(T _owner);
}
