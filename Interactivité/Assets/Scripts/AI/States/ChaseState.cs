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
        owner.NMA.speed = 8;
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

        if (Vector3.Distance(owner.transform.position, target.transform.position) <= 0.8f)
        {
            target.GetComponent<CharacterNetworkManager>().SendToServer_BackToSpawn();
            target = null;
            owner.DefaultState();
        }
    }
}
