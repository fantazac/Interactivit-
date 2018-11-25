using UnityEngine;

public class ChaseState : State<AI>
{
    private GameObject target;
    private AINetworkManager anm;

    public ChaseState(AINetworkManager anm, GameObject target)
    {
        this.anm = anm;
        this.target = target;
    }

    public override void EnterState(AI owner)
    {
        owner.NMA.speed = 8;
        owner.NMA.angularSpeed = 1000;
        owner.NMA.acceleration = 50;

        Debug.Log("[State].Chase (Owner: " + owner + ") - Entering Chase State. Chasing " + target.name);
    }

    public override void UpdateStateFromServer(float value)
    {
        target = null;
        anm.ai.OnTargetLost();
    }

    public override void UpdateState(AI owner)
    {
        if (!target)
        {
            anm.SendToServer_UpdateStateFromServer(0);
            return;
        }

        owner.NMA.destination = target.transform.position;

        if (Vector3.Distance(owner.transform.position, target.transform.position) <= 0.8f)
        {
            target.GetComponent<CharacterNetworkManager>().SendToServer_BackToSpawn();
            anm.SendToServer_UpdateStateFromServer(0);
        }
    }
}
