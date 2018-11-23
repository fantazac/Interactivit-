using UnityEngine;

public class RotateState : State<AI>
{
    private float timeBetweenRotations;
    private float rotationAngle;
    private float timeElapsedSinceLastRotation;

    public RotateState(float timeBetweenRotations = 2, float rotationAngle = 45)
    {
        this.timeBetweenRotations = timeBetweenRotations;
        this.rotationAngle = rotationAngle;
    }

    public override void EnterState(AI owner)
    {
        Debug.Log("[State].RotateState (Owner: " + owner + ") - Entering RotateState State.");
    }

    public override void UpdateState(AI owner)
    {
        if (owner.Sensor.CanSeeTarget)
        {
            owner.OnSeeTarget();
        }

        timeElapsedSinceLastRotation += Time.deltaTime;
        if (timeElapsedSinceLastRotation >= timeBetweenRotations)
        {
            timeElapsedSinceLastRotation -= timeBetweenRotations;
            owner.transform.Rotate(0, rotationAngle, 0);
        }
    }
}
