using UnityEngine;

public class ChaseState : State<AI>
{
    GameObject target;

    public ChaseState(GameObject target)
    {
        this.target = target;
    }

    public override void EnterState(AI owner)
    {
        owner.NMA.speed = 10;
        owner.NMA.angularSpeed = 1000;
        owner.NMA.acceleration = 50;

        Debug.Log("[State].Chase (Owner: " + owner + ") - Entering Chase State. Chasing " + target.name);
    }

    public override void UpdateState(AI owner)
    {
        if (!target)
        {
            owner.DefaultState();
            return;
        }

        owner.NMA.destination = target.transform.position;

        if (Vector3.Distance(owner.transform.position, target.transform.position) <= 0.5f)
        {
            // todo Respawn player
            owner.DefaultState();
        }

        Debug.DrawLine(owner.transform.position, target.transform.position, Color.cyan);
    }
}
