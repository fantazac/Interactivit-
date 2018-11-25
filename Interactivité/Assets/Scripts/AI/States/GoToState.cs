using UnityEngine;

public class GoToState : State<AI>
{
    private Vector3 destination;
    private float acceptableDistance;

    private AINetworkManager anm;

    public GoToState(AINetworkManager anm, Vector3 destination, float acceptableDistance)
    {
        this.anm = anm;
        if (StaticObjects.GameController.IsGameHost)
        {
            this.destination = destination;
            this.acceptableDistance = acceptableDistance;
        }
    }

    public override void EnterState(AI owner)
    {
        owner.NMA.speed = 3;
        owner.NMA.angularSpeed = 1000;
        owner.NMA.acceleration = 10;
    }

    public override void InitState()
    {
        if (StaticObjects.GameController.IsGameHost)
        {
            anm.SendToServer_UpdateStateFromServerWithVector(acceptableDistance, destination);
        }
    }

    public override void UpdateStateFromServer(float value) { }

    public override void UpdateStateFromServerWithVector(float value1, Vector3 value2)
    {
        acceptableDistance = value1;
        destination = value2;
        if (StaticObjects.GameController.GameIsActive)
        {
            StartState();
        }
    }

    public override void StartState()
    {
        anm.ai.NMA.SetDestination(destination);
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
