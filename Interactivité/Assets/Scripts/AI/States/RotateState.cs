using UnityEngine;

public class RotateState : State<AI>
{
    private float _timeBetweenRotations;
    private float _rotationAngle;
    private float timeElapsedSinceLastRotation;

    public RotateState(float timeBetweenRotations = 2f, float rotationAngle = 45f)
    {
        _timeBetweenRotations = timeBetweenRotations;
        _rotationAngle = rotationAngle;
    }
    
    public override void EnterState(AI _owner)
    {
        Debug.Log("[State].RotateState (Owner: " + _owner + ") - Entering RotateState State.");
    }

    public override void ExitState(AI _owner)
    {
        
    }

    public override void Update(AI _owner)
    {
        if (_owner.Sensor.CanSeeTarget)
            _owner.OnSeeTarget();
        
        timeElapsedSinceLastRotation += Time.deltaTime;
        if (timeElapsedSinceLastRotation >= _timeBetweenRotations)
        {
            timeElapsedSinceLastRotation -= _timeBetweenRotations;
            _owner.transform.Rotate(0f, _rotationAngle, 0f);
        }
    }
}
