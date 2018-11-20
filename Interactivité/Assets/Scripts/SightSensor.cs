using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSensor : MonoBehaviour
{
	public float ViewDistance = 16f;
	public float FieldOfView = 60f;
	
	private Color _gizmosDrawColor = Color.green;
	
	// Sensor status
	private GameObject _lastSeenTarget;
	private bool _canSeeTarget;
	
	
	#region Monobehaviour Callbacks
	
	private void Start()
	{
		ResetSensor();
	}

	void Update ()
	{
		if (_canSeeTarget)
			return;
		
		_gizmosDrawColor = Color.green;
		int numberOfTargets = SensorManager.TargetCount();
		
		//! Si trop lourd, ne verifier qu'un seul joueur par frame (index++ % _numberOfTargets)
		for (int index = 0; index < numberOfTargets; index++)
		{
			CheckIfTargetIsOnSight(SensorManager.GetTargetPosition(index));
		}
		
		Debug.DrawLine(transform.position, transform.position + transform.rotation * Quaternion.Euler(0f, FieldOfView / 2f, 0f) * Vector3.forward * ViewDistance, _gizmosDrawColor);
		Debug.DrawLine(transform.position, transform.position + transform.rotation * Quaternion.Euler(0f, -FieldOfView / 2f, 0f) * Vector3.forward * ViewDistance, _gizmosDrawColor);
	}

	#endregion

	public void SetupSensor(float _viewDistance, float _fieldOfView)
	{
		FieldOfView = _fieldOfView;
		ViewDistance = _viewDistance;
	}

	void CheckIfTargetIsOnSight(GameObject target)
	{
		if ((transform.position - target.transform.position).magnitude <= ViewDistance)
		{
			if (Vector3.Angle(target.transform.position - transform.position, transform.forward) <= FieldOfView / 2f)
			{
				_gizmosDrawColor = Color.yellow;
				
				RaycastHit hit;
				if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit))
				{
					if (hit.collider.CompareTag(SensorManager._TagToLookFor))
					{
						_lastSeenTarget = target;
						_canSeeTarget = true;
						return;
					}
						
					Debug.DrawLine(transform.position, hit.point, _gizmosDrawColor);
				}
			}
		}
		_canSeeTarget = false;
	}
		
	public void ResetSensor()
	{
		_canSeeTarget = false;
	}

	public bool CanSeeTarget()
	{
		return _canSeeTarget;
	}

	public GameObject GetTarget()
	{
		return _lastSeenTarget;
	}

}
