using UnityEngine;

public class SightSensor : MonoBehaviour
{
    public bool CanSeeTarget { get; protected set; }
    public GameObject Target { get; protected set; }

    public float ViewDistance = 16;
    public float FieldOfView = 60;

    #region Monobehaviour Callbacks

    private void Start()
    {
        Target = StaticObjects.CharacterNetworkManager.gameObject;
    }

    private void Update()
    {
        CanSeeTarget = TargetIsInSight();

        if (CanSeeTarget)
        {
            Debug.DrawLine(transform.position, Target.transform.position, Color.cyan);
        }

        // Debug draw sight range
        Debug.DrawLine(transform.position, transform.position + transform.rotation * Quaternion.Euler(0f, FieldOfView / 2f, 0f) * Vector3.forward * ViewDistance, Color.green);
        Debug.DrawLine(transform.position, transform.position + transform.rotation * Quaternion.Euler(0f, -FieldOfView / 2f, 0f) * Vector3.forward * ViewDistance, Color.green);
    }

    #endregion

    public void SetupSensor(float viewDistance, float fieldOfView)
    {
        FieldOfView = fieldOfView;
        ViewDistance = viewDistance;
    }

    private bool TargetIsInSight()
    {
        return Target != null && TargetIsInViewDistance() && TargetIsInFieldOfView() && TargetIsDirectlyInSight();
    }

    private bool TargetIsInViewDistance()
    {
        return (transform.position - Target.transform.position).magnitude <= ViewDistance;
    }

    private bool TargetIsInFieldOfView()
    {
        return Vector3.Angle(Target.transform.position - transform.position, transform.forward) <= FieldOfView * 0.5f;
    }

    private bool TargetIsDirectlyInSight()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, Target.transform.position - transform.position, out hit) && hit.collider.gameObject == Target;
    }
}
