using UnityEngine;

public abstract class Behaviour : ScriptableObject
{
    public abstract State<AI> GetDefaultBehaviour();
}
