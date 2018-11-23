using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State<AI>
{
	GameObject _target;

	public ChaseState(GameObject target)
	{
		_target = target;
	}
	
	public override void EnterState(AI _owner)
	{
		_owner.NMA.speed = 10f;
		_owner.NMA.angularSpeed = 1000f;
		_owner.NMA.acceleration = 50;
		
		Debug.Log("Now chasing " + _target.name);
	}

	public override void ExitState(AI _owner)
	{
		
	}

	public override void Update(AI _owner)
	{
		if (!_target)
		{
			_owner.DefaultState();
			return;
		}

		_owner.NMA.destination = _target.transform.position;

		if (Vector3.Distance(_owner.transform.position, _target.transform.position) <= .5)
		{
			// todo Respawn player
			_owner.DefaultState();
		}
		
		Debug.DrawLine(_owner.transform.position, _target.transform.position, Color.cyan);
	}
}
