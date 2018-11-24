using UnityEngine;

public class RotateState : State<AI>
{
    private float rotationSpeed;
    private float timeElapsedSinceLastRotation;

    public RotateState(float rotationSpeed = 45, float maxModifierOffset = 20f)
    {
        this.rotationSpeed = rotationSpeed + Random.Range(-maxModifierOffset, maxModifierOffset);
        this.rotationSpeed *= Random.value <= .5f ? 1 : -1;
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
        
        owner.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
