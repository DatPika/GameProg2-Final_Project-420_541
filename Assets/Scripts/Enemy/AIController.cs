using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public StateMachine StateMachine { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; } // Not needed since we're not using animations
    public Transform[] Waypoints;
    public Transform Player;
    public GameObject PlayerPrefab;
    public float SightRange = 10f;
    public float AttackRange = 2f; // New attack range variable
    public LayerMask PlayerLayer;
    public StateType currentState;

    private bool isPlayerInvisible;
    private AudioSource audioChase;

    void Start()
    {
        audioChase = GetComponent<AudioSource>();
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        StateMachine = new StateMachine();
        StateMachine.AddState(new IdleState(this));
        StateMachine.AddState(new PatrolState(this));
        StateMachine.AddState(new ChaseState(this));
        StateMachine.AddState(new AttackState(this)); // Add the new AttackState

        StateMachine.TransitionToState(StateType.Idle);
    }

    void Update()
    {
        isPlayerInvisible = PlayerPrefab.GetComponent<PlayerMechanics>().isInvisible;
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        
        if (IsPlayerInAttackRange())
            StateMachine.TransitionToState(StateType.Attack);
        else if (CanSeePlayer())
            StateMachine.TransitionToState(StateType.Chase);
        else
            StateMachine.TransitionToState(StateType.Patrol);
        
        StateMachine.Update();
    }

    public bool CanSeePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        if (distanceToPlayer <= SightRange && !isPlayerInvisible)
        {
            if (!audioChase.isPlaying) {
                audioChase.Play();
            }
            return true;
        }
        audioChase.Stop();
        return false;
    }

    // New method to check if the AI is within attack range
    public bool IsPlayerInAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        return distanceToPlayer <= AttackRange;
    }
}
