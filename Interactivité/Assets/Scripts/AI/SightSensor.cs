using UnityEngine;

public class SightSensor : MonoBehaviour
{
    public bool CanSeeTarget { get; protected set; }
    public GameObject Target { get; protected set; }

    public float ViewDistance = 16;
    public float FieldOfView = 60;
    
    AI owner;
    AIManager aiManagerInstance;
    Transform preferedTarget;
    Transform tmpTarget;
    bool hasSeenTargetThisFrame;

    #region Monobehaviour Callbacks

    private void Start()
    {
        aiManagerInstance = AIManager.Instance;
        owner = GetComponent<AI>();
    }

    private void Update()
    {
        if (!owner.ShouldLookForTargets)
            return;

        hasSeenTargetThisFrame = false;
        for (int index = 0; index < aiManagerInstance.targets.Length; index++)
        {
            tmpTarget = aiManagerInstance.targets[index];

            if (TargetIsInSight())
            {
                if (!hasSeenTargetThisFrame)
                {
                    Target = tmpTarget.gameObject;
                    hasSeenTargetThisFrame = true;
                }
                else if ((transform.position - tmpTarget.position).magnitude < (transform.position - Target.transform.position).magnitude)
                    Target = tmpTarget.gameObject;
            }
        }
        CanSeeTarget = hasSeenTargetThisFrame;

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
        return tmpTarget != null && TargetIsInViewDistance() && TargetIsInFieldOfView() && TargetIsDirectlyInSight();
    }

    private bool TargetIsInViewDistance()
    {
        return (transform.position - tmpTarget.position).magnitude <= ViewDistance;
    }

    private bool TargetIsInFieldOfView()
    {
        return Vector3.Angle(tmpTarget.position - transform.position, transform.forward) <= FieldOfView * 0.5f;
    }

    private bool TargetIsDirectlyInSight()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, tmpTarget.position - transform.position, out hit) && hit.collider.gameObject == tmpTarget.gameObject;
    }
}
