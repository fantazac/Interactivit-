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
		Debug.Log("Now chasing " + _target.name);
	}

	public override void ExitState(AI _owner)
	{
		
	}

	public override void Update(AI _owner)
	{
		Debug.DrawLine(_owner.transform.position, _target.transform.position, Color.cyan);
	}
}
