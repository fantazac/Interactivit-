using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToState : State<AI>
{
	private Vector3 _destination;
	private float _acceptableDistance;
	
	
	public GoToState(Vector3 destination, float acceptableDistance)
	{
		_destination = destination;
		_acceptableDistance = acceptableDistance;
	}
	
	public override void EnterState(AI _owner)
	{
		_owner.NMA.speed = 3f;
		_owner.NMA.angularSpeed = 1000f;
		_owner.NMA.acceleration = 10f;
		
		_owner.NMA.SetDestination(_destination);
	}

	public override void ExitState(AI _owner)
	{
		
	}

	public override void Update(AI _owner)
	{
		if (_owner.Sensor.CanSeeTarget)
			_owner.OnTargetFound();
		
		if (_owner.NMA.remainingDistance <= _acceptableDistance)
			_owner.DefaultState();
	}
}
