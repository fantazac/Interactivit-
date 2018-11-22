using UnityEngine;

public class SightSensor : MonoBehaviour
{
	public bool CanSeeTarget { get; protected set; }
	public GameObject Target { get; protected set; }
	
	public float ViewDistance = 16f;
	public float FieldOfView = 60f;
	
	
	#region Monobehaviour Callbacks
	
	private void Start()
	{
		Target = StaticObjects.CharacterNetworkManager.gameObject;
	}

	void Update ()
	{
		CanSeeTarget = TargetIsOnSight();
		
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

	bool TargetIsOnSight()
	{
		if (Target == null)
			return false;
			
		if ((transform.position - Target.transform.position).magnitude <= ViewDistance) // target is in view distance
		{
			if (Vector3.Angle(Target.transform.position - transform.position, transform.forward) <= FieldOfView / 2f) // target is in field of view
			{				
				RaycastHit hit;
				if (Physics.Raycast(transform.position, Target.transform.position - transform.position, out hit))
				{
					if (hit.collider.gameObject == Target) // target is not hiding behind a wall
						return true;
				}
			}
		}
		return false;
	}
}
