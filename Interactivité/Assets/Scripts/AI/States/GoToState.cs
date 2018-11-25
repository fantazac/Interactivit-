using UnityEngine;

public class GoToState : State<AI>
{
    private Vector3 destination;
    private float acceptableDistance;

    public GoToState(Vector3 destination, float acceptableDistance)
    {
        this.destination = destination;
        this.acceptableDistance = acceptableDistance;
    }

    public override void EnterState(AI owner)
    {
        owner.NMA.speed = 3;
        owner.NMA.angularSpeed = 1000;
        owner.NMA.acceleration = 10;

        owner.NMA.SetDestination(destination);
    }

    public override void UpdateStateFromServer(float value)
    {

    }

    public override void UpdateState(AI owner)
    {
        if (owner.Sensor.CanSeeTarget)
        {
            owner.OnTargetFound();
        }

        if (owner.NMA.remainingDistance <= acceptableDistance)
        {
            owner.DefaultState();
        }
    }
}
