using UnityEngine;

public class AI : MonoBehaviour
{
    public StateMachine<AI> StateMachine { get; protected set; }
    public SightSensor Sensor { get; protected set; }
    public bool Active = true;
    
    [Header("AI Behaviour")]
    public float ViewDistance = 6f; 
    public float FieldOfView = 60f;
    public float RotationRate = .2f;
    public float RotationAngle = 20f;
    public float ReactionTime = .75f;

    private void Start()
    {
        Sensor = gameObject.AddComponent<SightSensor>();
        Sensor.SetupSensor(ViewDistance, FieldOfView);
		
        StateMachine = new StateMachine<AI>(this, new RotateState(RotationRate, RotationAngle));
    }

    private void Update()
    {
        if (Active)
            StateMachine.Update();
    }

    public void OnSeeTarget()
    {
        Debug.Log("[AI]." + name + " - What was that? I saw something...");
        StateMachine.ChangeState(new LookAtState(ReactionTime));
    }

    public void OnTargetFound()
    {
        Debug.Log("[AI]." + name + " - Found target! Now chasing the target.");
        StateMachine.ChangeState(new ChaseState(Sensor.Target));
    }
	
    public void OnTargetLost()
    {
        Debug.Log("[AI]." + name + " - Lost target. Returning to default state.");
        StateMachine.ChangeState(new RotateState(RotationRate, RotationAngle));
    }
}

