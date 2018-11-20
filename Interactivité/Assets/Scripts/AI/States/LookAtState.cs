using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtState : State<AI>
{
	float _reactionTime;
	GameObject _target;
	
	
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
		{
			// todo: change state back to rotation
		}

		// todo check if we can still see the target
		RaycastHit hit;
		bool canSeeTarget = false;
		
		if (Physics.Raycast(_owner.transform.position, _target.transform.position - _owner.transform.position, out hit))
		{
			if (hit.collider.CompareTag(SensorManager._TagToLookFor))
			{
				Debug.DrawLine(_owner.transform.position, hit.point, Color.cyan);
				canSeeTarget = true;
			}
		}
		
		Debug.DrawLine(_owner.transform.position, hit.point, canSeeTarget ? Color.green : Color.red);
		// Handle reaction time
		if (canSeeTarget)
		{
			
		}
		
		// todo rotate if we do
		Vector3 LookAtPosition = _target.transform.position;
		LookAtPosition.y = _owner.transform.position.y;
		_owner.transform.LookAt(LookAtPosition);
	}
}
