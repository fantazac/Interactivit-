using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSensor : MonoBehaviour
{

	[Tooltip("The radius from which the sensor will react when it sees something")]
	public float SensorRadius = 16f;
	
	[Tooltip("The field of view in angle")]
	public float FieldOfView = 60f;
	
	[SerializeField] float _reactionTime = 1f;
	
	float _currentAlertLevel;
	private Color _gizmosDrawColor = Color.green;

	private GameObject _lastSeenTarget;
	private GameObject _foundTraget;
	private bool _hasFoundTarget;
	
	
	#region Monobehaviour Callbacks

	private void Start()
	{
		ResetSensor();
	}

	void Update ()
	{
		if (_hasFoundTarget)
			return;
		
		_gizmosDrawColor = Color.green;
		bool hasSeenTarget = false;
		int numberOfTargets = SensorManager.TargetCount();
		
		//! Si trop lourd, ne verifier qu'un seul joueur par frame (index++ % _numberOfTargets)
		for (int index = 0; index < numberOfTargets; index++)
		{
			if (TargetIsOnSight(SensorManager.GetTargetPosition(index)))
			{
				hasSeenTarget = true;
			}
		}
		
		// Handle reaction time
		_currentAlertLevel += hasSeenTarget ? Time.deltaTime : -Time.deltaTime / 5f;
		if (_currentAlertLevel >= _reactionTime)
		{
			_hasFoundTarget = true;
			_foundTraget = _lastSeenTarget;
		}
		_currentAlertLevel = Mathf.Clamp(_currentAlertLevel, 0f, _reactionTime);
		
		
		// Debug draw sensor field of view
		Debug.DrawLine(transform.position, transform.position + transform.rotation * Quaternion.Euler(0f, FieldOfView / 2f, 0f) * Vector3.forward * SensorRadius, _gizmosDrawColor);
		Debug.DrawLine(transform.position, transform.position + transform.rotation * Quaternion.Euler(0f, -FieldOfView / 2f, 0f) * Vector3.forward * SensorRadius, _gizmosDrawColor);
	}

	#endregion

	bool TargetIsOnSight(GameObject target)
	{
		if ((transform.position - target.transform.position).magnitude <= SensorRadius)
		{
			if (Vector3.Angle(target.transform.position - transform.position, transform.forward) <= FieldOfView / 2f)
			{
				_gizmosDrawColor = Color.yellow;
				
				RaycastHit hit;
				if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit))
				{
					if (hit.collider.CompareTag(SensorManager._TagToLookFor))
					{
						_gizmosDrawColor = Color.red;
						
						Debug.DrawLine(transform.position, hit.point, _gizmosDrawColor);

						_lastSeenTarget = target;
						return true;
					}
						
					Debug.DrawLine(transform.position, hit.point, _gizmosDrawColor);
				}
			}
		}
		return false;
	}
		
	public void ResetSensor()
	{
		_hasFoundTarget = false;
		_currentAlertLevel = 0f;
	}

	public bool HasFoundTarget()
	{
		return _hasFoundTarget;
	}

	public GameObject GetTarget()
	{
		return _foundTraget;
	}

}
