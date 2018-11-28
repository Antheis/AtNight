using Player;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class BaseEnnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float     _walkSpeed = 10f;
    [SerializeField] private float     _runSpeed = 20f;

    private readonly int               AttackRange;

    protected Vector3                  Forward;
    protected bool                     Walking;
    protected bool                     ChasePlayer;
    
    protected FirstPersonController    Personnage;
    protected NavMeshAgent             CurrentNavMeshAgent;
    
    protected Animator                 CurrentAnimator;
    protected readonly int             HashSpeed = Animator.StringToHash("Speed");
    protected readonly int             HashAttack = Animator.StringToHash("Speed");
    
    private void Awake()
    {
        CurrentAnimator = GetComponent<Animator>();
        CurrentNavMeshAgent = GetComponent<NavMeshAgent>();
        Personnage = FindObjectOfType<FirstPersonController>();
    }
    
    private void Update()
    {
        if (ChasePlayer)
            ChasePlayerLogic();
        else
        {
            transform.forward = Forward;
        }
    }

    private void ChasePlayerLogic()
    {
        if ((Personnage.transform.position - transform.position).magnitude < AttackRange)
        {
            ; //ToDo: Attack
        }
        else if ((Personnage.transform.position - CurrentNavMeshAgent.destination).magnitude > 0.1f)
            CurrentNavMeshAgent.destination = Personnage.transform.position;
    }
}
