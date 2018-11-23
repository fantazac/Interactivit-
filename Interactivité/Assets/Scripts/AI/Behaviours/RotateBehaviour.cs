using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Rotate")]
public class RotateBehaviour : Behaviour
{
    public float RotationRate;
    public float RotationAngle;

    public override State<AI> GetDefaultBehaviour()
    {
        return new RotateState(RotationRate, RotationAngle);
    }
}
