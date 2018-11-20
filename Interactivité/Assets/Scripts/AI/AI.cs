using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AI : MonoBehaviour
{
	public StateMachine<AI> StateMachine { get; protected set; }
	public SightSensor Sensor;

	[Header("AI Behaviour")]
	public float ViewDistance = 6f; 
	public float FieldOfView = 30f;
	public float ReactionTime = 5f;
	
	
	// Use this for initialization
	void Start ()
	{
		Sensor = gameObject.AddComponent<SightSensor>();
		Sensor.SetupSensor(ViewDistance, FieldOfView);
		
		StateMachine = new StateMachine<AI>(this, new RotateState(.5f, 30f));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Sensor.CanSeeTarget())
			StateMachine.ChangeState(new LookAtState(Sensor.GetTarget(), ReactionTime));
		
		StateMachine.Update();
	}

	public void OnTargetFound(GameObject target)
	{
		StateMachine.ChangeState(new ChaseState(target));
	}
	
	public void OnTargetLost()
	{
		StateMachine.ChangeState(new RotateState());
	}
	
}
