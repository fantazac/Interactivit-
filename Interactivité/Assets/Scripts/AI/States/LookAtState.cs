using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtState : State<AI>
{
	GameObject _target;
	float _reactionTime;
	float _currentAlertLevel;
	
	public LookAtState(GameObject target, float reactionTime)
	{
		_reactionTime = reactionTime;
		_target = target;
	}
	
	public override void EnterState(AI _owner)
	{
		
	}

	public override void ExitState(AI _owner)
	{
		
	}

	public override void Update(AI _owner)
	{
		if (_target == null)
			_owner.OnTargetLost();

		RaycastHit hit;
		bool canSeeTarget = false;
		
		if (Physics.Raycast(_owner.transform.position, _target.transform.position - _owner.transform.position, out hit))
		{
			if (hit.collider.CompareTag(SensorManager._TagToLookFor))
			{
				canSeeTarget = true;
			}
		}
		
		Debug.DrawLine(_owner.transform.position, hit.point, canSeeTarget ? Color.green : Color.red);
		// Handle reaction time
		if (canSeeTarget)
		{
			_currentAlertLevel += canSeeTarget ? Time.deltaTime : -Time.deltaTime / 5f;
			if (_currentAlertLevel >= _reactionTime)
			{
				_owner.OnTargetFound(_target);
			}
			_currentAlertLevel = Mathf.Clamp(_currentAlertLevel, 0f, _reactionTime);
			
			//todo fix
			Debug.Log(_currentAlertLevel + " / " + _reactionTime);
		}
		
		Vector3 LookAtPosition = _target.transform.position;
		LookAtPosition.y = _owner.transform.position.y;
		_owner.transform.LookAt(LookAtPosition);
	}
}
