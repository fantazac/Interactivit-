using UnityEngine;

public class RotateState : State<AI>
{
    private float rotationSpeed;
    private float maxModifierOffset;
    private float timeElapsedSinceLastRotation;

    public RotateState(AINetworkManager anm, float rotationSpeed = 40, float maxModifierOffset = 20)
    {
        if (StaticObjects.GameController.IsGameHost)
        {
            this.rotationSpeed = rotationSpeed;
            this.maxModifierOffset = maxModifierOffset;
            anm.SendToServer_UpdateStateFromServer(GetRotationSpeed());
        }
    }

    private float GetRotationSpeed()
    {
        float rotationSpeedToSend = rotationSpeed + Random.Range(-maxModifierOffset, maxModifierOffset);
        rotationSpeedToSend *= Random.value <= 0.5f ? 1 : -1;
        rotationSpeed = 0;
        return rotationSpeedToSend;
    }

    public override void UpdateStateFromServer(float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
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
