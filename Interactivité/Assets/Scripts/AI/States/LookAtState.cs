using UnityEngine;

public class LookAtState : State<AI>
{
	float _reactionTime;
	float _currentAlertLevel;
	
	public LookAtState(float reactionTime)
	{
		_reactionTime = reactionTime;
	}
	
	public override void EnterState(AI _owner)
	{
		Debug.Log("[State].LookAt (Owner: " + _owner + ") - Entering LookAt State.");
	}

	public override void ExitState(AI _owner)
	{
		
	}

	public override void Update(AI _owner)
	{
		if (_owner.Sensor.CanSeeTarget)
		{
			// Handle reaction time
			_currentAlertLevel += Time.deltaTime;
			if (_currentAlertLevel >= _reactionTime)
				_owner.OnTargetFound();
			
			// Rotate to target
			Vector3 lookAtPosition = _owner.Sensor.Target.transform.position;
			lookAtPosition.y = _owner.transform.position.y;
			_owner.transform.LookAt(lookAtPosition);
		}
		else
		{
			// Handle reaction time
			_currentAlertLevel -= Time.deltaTime / 5f;
			if (_currentAlertLevel <= -.2f)
				_owner.OnTargetLost();
		}
	}
}
