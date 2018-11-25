using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Rotate")]
public class RotateBehaviour : Behaviour
{
    public float RotationSpeed;
    public float MaxModifierOffset;

    public override State<AI> GetDefaultBehaviour(AINetworkManager anm)
    {
        return new RotateState(anm, RotationSpeed, MaxModifierOffset);
    }
}
