using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Patrol")]
public class PatrolBehaviour : Behaviour
{
    public override State<AI> GetDefaultBehaviour()
    {
        Vector3 targetPosition = WaypointManager.GetRandomWaypoint();
        return new GoToState(targetPosition, 0.5f);
    }
}
