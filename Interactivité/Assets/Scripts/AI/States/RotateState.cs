using UnityEngine;

public class RotateState : State<AI>
{
    private float _timeBetweenRotations;
    private float _rotationAngle;
    private float timeElapsedSinceLastRotation;


    public RotateState(float TimeBetweenRotations = 2f, float RotationAngle = 45f)
    {
        _timeBetweenRotations = TimeBetweenRotations;
        _rotationAngle = RotationAngle;
    }
    
    public override void EnterState(AI _owner)
    {
        
    }

    public override void ExitState(AI _owner)
    {
        
    }

    public override void Update(AI _owner)
    {
        timeElapsedSinceLastRotation += Time.deltaTime;
        if (timeElapsedSinceLastRotation >= _timeBetweenRotations)
        {
            timeElapsedSinceLastRotation -= _timeBetweenRotations;
            _owner.transform.Rotate(0f, _rotationAngle, 0f);
        }
    }
}
