using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	public StateMachine<AI> StateMachine { get; protected set; }
	public SightSensor Sensor;
	
	// Use this for initialization
	void Start ()
	{
		StateMachine = new StateMachine<AI>(this, new RotateState(.5f, 30f));
		
		Sensor = gameObject.GetComponent<SightSensor>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Sensor.HasFoundTarget())
			StateMachine.ChangeState(new ChaseState(Sensor.GetTarget()));
		
		StateMachine.Update();
	}

	public void OnSeeTarget(GameObject target)
	{
		
	}
	
	
}
