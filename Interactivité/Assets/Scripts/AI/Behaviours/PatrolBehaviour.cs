using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Patrol")]
public class PatrolBehaviour : Behaviour
{
    public override State<AI> GetDefaultBehaviour(AINetworkManager anm)
    {
        Vector3 targetPosition = WaypointManager.GetRandomWaypoint();
        return new GoToState(anm, targetPosition, 0.5f);
    }
}
