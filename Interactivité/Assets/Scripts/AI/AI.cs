using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public StateMachine<AI> StateMachine { get; protected set; }
    public SightSensor Sensor { get; protected set; }
    public NavMeshAgent NMA { get; protected set; }
    public bool ShouldLookForTargets { get; set; }

    [Header("AI Behaviour")]
    public float ViewDistance = 6;
    public float FieldOfView = 60;
    public float ReactionTime = 0.75f;
    public Behaviour DefaultBehaviour;

    private Vector3 defaultPosition;

    private void Start()
    {
        Sensor = gameObject.AddComponent<SightSensor>();
        Sensor.SetupSensor(ViewDistance, FieldOfView);

        NMA = gameObject.GetComponent<NavMeshAgent>();

        defaultPosition = transform.position;
        StateMachine = new StateMachine<AI>(this, DefaultBehaviour.GetDefaultBehaviour());
    }

    private void Update()
    {
        StateMachine.UpdateStateMachine();
    }

    public void SetBehaviour(Behaviour behaviour)
    {
        DefaultBehaviour = behaviour;
        if (StateMachine == null || NMA == null)
            Start();
        DefaultState();
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
        StateMachine = new StateMachine<AI>(this, new GoToState(defaultPosition, .3f));
    }
}

