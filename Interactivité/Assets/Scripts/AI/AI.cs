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

    private AINetworkManager anm;

    private Vector3 defaultPosition;

    private void Awake()
    {
        Sensor = gameObject.AddComponent<SightSensor>();
        Sensor.SetupSensor(ViewDistance, FieldOfView);

        NMA = GetComponent<NavMeshAgent>();

        defaultPosition = transform.position;

        anm = GetComponent<AINetworkManager>();
        StateMachine = new StateMachine<AI>(this);
        StateMachine.ChangeState(DefaultBehaviour.GetDefaultBehaviour(anm));
    }

    private void Update()
    {
        if (StaticObjects.GameController.GameIsActive)
        {
            StateMachine.UpdateStateMachine();
        }
    }

    public void SetBehaviour(Behaviour behaviour)
    {
        DefaultBehaviour = behaviour;
        if (StateMachine == null || NMA == null)
        {
            Awake();
        }
        DefaultState();
    }

    public void DefaultState()
    {
        StateMachine.ChangeState(DefaultBehaviour.GetDefaultBehaviour(anm));
    }

    public void OnSeeTarget()
    {
        Debug.Log("[AI]." + name + " - What was that? I saw something...");
        StateMachine.ChangeState(new LookAtState(ReactionTime));
    }

    public void OnTargetFound()
    {
        Debug.Log("[AI]." + name + " - Found target! Now chasing the target.");
        StateMachine.ChangeState(new ChaseState(anm, Sensor.Target));
    }

    public void OnTargetLost()
    {
        Debug.Log("[AI]." + name + " - Lost target. Returning to default state.");
        StateMachine.ChangeState(new GoToState(anm, defaultPosition, 0.3f));
    }
}

