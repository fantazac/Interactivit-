using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Rotate")]
public class RotateBehaviour : Behaviour
{
    public float RotationSpeed;
    public float MaxModifierOffset;

    public override State<AI> GetDefaultBehaviour()
    {
        return new RotateState(RotationSpeed, MaxModifierOffset);
    }
}
