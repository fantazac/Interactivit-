using UnityEngine;

public class LookAtState : State<AI>
{
    private float reactionTime;
    private float currentAlertLevel;

    public LookAtState(float reactionTime)
    {
        this.reactionTime = reactionTime;
    }

    public override void EnterState(AI owner)
    {
        Debug.Log("[State].LookAt (Owner: " + owner + ") - Entering LookAt State.");
    }

    public override void UpdateState(AI owner)
    {
        bool canSeeTarget = owner.Sensor.CanSeeTarget;
        HandleReactionTime(owner, canSeeTarget);
        if (canSeeTarget)
        {
            RotateTowardsTarget(owner);
        }
    }

    private void HandleReactionTime(AI owner, bool canSeeTarget)
    {
        if (canSeeTarget)
        {
            currentAlertLevel += Time.deltaTime;
            if (currentAlertLevel >= reactionTime)
            {
                owner.OnTargetFound();
            }
        }
        else
        {
            currentAlertLevel -= Time.deltaTime * 0.5f;
            if (currentAlertLevel <= -0.2f)
            {
                owner.OnTargetLost();
            }
        }
    }

    private void RotateTowardsTarget(AI owner)
    {
        Vector3 lookAtPosition = owner.Sensor.Target.transform.position;
        lookAtPosition.y = owner.transform.position.y;
        owner.transform.LookAt(lookAtPosition);
    }
}
