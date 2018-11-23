using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public StateMachine<AI> StateMachine { get; protected set; }
    public SightSensor Sensor { get; protected set; }
    public NavMeshAgent NMA { get; protected set; }
    public bool Active = true;
    
    [Header("AI Behaviour")]
    public float ViewDistance = 6f; 
    public float FieldOfView = 60f;
    public float ReactionTime = .75f;
    public Behaviour DefaultBehaviour;

    private Vector3 _defaultPosition;
    

    private void Start()
    {
        Sensor = gameObject.AddComponent<SightSensor>();
        Sensor.SetupSensor(ViewDistance, FieldOfView);

        NMA = gameObject.GetComponent<NavMeshAgent>();

        _defaultPosition = transform.position;
        StateMachine = new StateMachine<AI>(this, DefaultBehaviour.GetDefaultBehaviour());
    }

    private void Update()
    {
        if (Active)
            StateMachine.Update();
    }


    public void DefaultState()
    {
        StateMachine.ChangeState(DefaultBehaviour.GetDefaultBehaviour());
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
        StateMachine = new StateMachine<AI>(this, new GoToState(_defaultPosition, .3f));
    }
}

