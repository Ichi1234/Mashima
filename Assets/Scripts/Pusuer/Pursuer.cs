using System;
using UnityEngine;
using UnityEngine.AI;

public class Pursuer : Entity
{
    [Header("Pursuer MoveSpeed")]
    [SerializeField] private float moveSpeed = 4.6f;
    [SerializeField] private float chaseSpeedMultiplier = 1.25f;
    [SerializeField] private float runSpeedMultiplier = 1.5f;
    [SerializeField] private float slowDownMultiplier = 0.55f;
    [SerializeField] private float partialSlowDownMultiplier = 0.85f;
    [SerializeField] private float slowDownDuration = 2;

    [Header("Pursuer Eyes")]
    [SerializeField] private float playerDetectionRange;
    [SerializeField] private LayerMask detectionRaycastMask;
    [SerializeField] private Transform pursuerEyes;
    [SerializeField] private float eyesRotationSpeed = 6f;
    [SerializeField] private float horizontalAngle = 90f;
    [SerializeField] private float verticalAngle = 60f;

    [SerializeField] private NavMeshAgent agent;

    [Header("Animation")]
    [SerializeField] private PursuerAnimationController animController;

    [Header("General")]
    [SerializeField] private string screechName = "pursuer-screech";

    public Action OnReachedTheDesitnation;

    public bool IsSeeingPlayer = false;

    private float slowdownTimer;
    private bool isSlowing = false;

    public Pursuer_IdleState IdleState { get; private set; }
    public Pursuer_PatrolState PatrolState { get; private set; }
    public Pursuer_ChaseState ChaseState { get; private set; }
    public Pursuer_LosePlayerState LosePlayerState { get; private set; }
    public Pursuer_RoarState RoarState { get; private set; }

    private CapsuleCollider playerDetectionCollider;

    private Vector3 initialPos;
    public float ChaseSpeedMultiplier => chaseSpeedMultiplier;
    public float RunSpeedMultiplier => runSpeedMultiplier;
    public PursuerAnimationController Animation => animController;
    public AudioSource AudioSource => audioSource;


    protected override void Awake()
    {
        base.Awake();

        agent.speed = moveSpeed;
       
        IdleState = new Pursuer_IdleState(this, stateMachine);
        PatrolState = new Pursuer_PatrolState(this, stateMachine);
        ChaseState = new Pursuer_ChaseState(this, stateMachine);
        LosePlayerState = new Pursuer_LosePlayerState(this, stateMachine);
        LosePlayerState = new Pursuer_LosePlayerState(this, stateMachine);
        RoarState = new Pursuer_RoarState(this, stateMachine);

        initialPos = transform.position;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnPlayerDeath += PlayerDeath;

        animController.OnFootSteped += PlayFootStepSound;
    }

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.Log("How tf I am null");
        }

        playerDetectionCollider = GameManager.Instance.GetPlayerDetectionCollider();

        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        IsSeeingPlayer = PlayerDetection(out RaycastHit hit);

        base.Update();

        SlamTheDoorOpen();

        RecoverFromSlowdown();

        agent.speed = moveSpeed * moveSpeedMultiplier;

        if (!agent.pathPending && agent.remainingDistance <= 0.02f)
        {
            OnReachedTheDesitnation?.Invoke();
        }


        Debug.Log(IsSeeingPlayer);


        Vector3 playerPos = playerDetectionCollider.transform.position;

        if (Vector3.Distance(playerPos, transform.position) < 1.5f)
        {
            GameManager.Instance.OnPlayerDeath?.Invoke();
        }

    }

    private void RecoverFromSlowdown()
    {
        if (isSlowing && slowdownTimer <= 0)
        {
            switch (stateMachine.currentState)
            {
                case Pursuer_PatrolState:
                    ResetMoveSpeedMultiplier();
                    break;
                case Pursuer_ChaseState:
                    SetMoveSpeedMultiplier(ChaseSpeedMultiplier);
                    break;
                case Pursuer_LosePlayerState:
                    ResetMoveSpeedMultiplier();
                    break;
                default:
                    ResetMoveSpeedMultiplier();
                    break;
            }

            isSlowing = false;
        }

        slowdownTimer -= Time.deltaTime;
    }

    private void SlamTheDoorOpen()
    {
        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out RaycastHit hit, 1.5f))
        {
            Door door = hit.transform.GetComponent<Door>();
            if (door == null) return;

            bool wasFullyShut = !door.IsOpen && !door.IsPartiallyOpen;
            bool wasPartiallyOpen = door.IsPartiallyOpen;
            bool needToOpen = !door.IsOpen || door.IsPartiallyOpen;

            if (needToOpen)
            {
                door.OpenWithForce(GameManager.Instance.DoorSlamForce);


                if (!isSlowing)
                {
                    if (wasFullyShut)
                    {
                        SetMoveSpeedMultiplier(slowDownMultiplier);
                       
                    }
                    else if (wasPartiallyOpen)
                    {
                        SetMoveSpeedMultiplier(partialSlowDownMultiplier); 
                        
                    }

                    slowdownTimer = slowDownDuration;
                    isSlowing = true;
                }
            }
        }
    }

    private void OnDisable() => GameManager.Instance.OnPlayerDeath -= PlayerDeath;

    private void PlayerDeath()
    {

        animController.ResetAllAnimation();

        stateMachine.ChangeState(IdleState);

        bool warped = agent.Warp(initialPos);

        ResumeMovement();
    }

    private bool PlayerDetection(out RaycastHit hit)
    {
        hit = default;

        Vector3 playerHead =
            playerDetectionCollider.bounds.center +
            Vector3.up * playerDetectionCollider.bounds.extents.y;
        
        Vector3 directionToPlayer = (playerHead - pursuerEyes.position).normalized;
        
        float distanceToPlayer = Vector3.Distance(pursuerEyes.position, playerHead);

        if (distanceToPlayer > playerDetectionRange)
        {
            return false;
        }

        // Horizontal angle check (flatten Y)
        Vector3 forwardFlat = new Vector3(pursuerEyes.forward.x, 0, pursuerEyes.forward.z).normalized;
        Vector3 dirToPlayerFlat = new Vector3(directionToPlayer.x, 0, directionToPlayer.z).normalized;
        float horizontalAngleToPlayer = Vector3.Angle(forwardFlat, dirToPlayerFlat);

        if (horizontalAngleToPlayer > horizontalAngle / 2f)
        {
            return false;
        }

        // Vertical angle check
        Vector3 forwardVertical = 
            Vector3.ProjectOnPlane(pursuerEyes.forward, pursuerEyes.right).normalized;

        Vector3 dirToPlayerVertical =
            Vector3.ProjectOnPlane(directionToPlayer, pursuerEyes.right).normalized;

        float verticalAngleToPlayer =
            Vector3.Angle(forwardVertical, dirToPlayerVertical);
        
        
        if (verticalAngleToPlayer > verticalAngle / 2f)
        {
            return false;

        }

        // CHECK IS SMTH BLOCK PLAYER!?!?!?
        if (Physics.Raycast(
                pursuerEyes.position,
                directionToPlayer,
                out hit,
                distanceToPlayer,
                detectionRaycastMask)
            )
        {
            return hit.collider == playerDetectionCollider;
        }

        return false;

    }

    private void OnDrawGizmos()
    {
        if (pursuerEyes == null)
            return;

        Gizmos.color = Color.red;

        float halfHorizontal = horizontalAngle * 0.5f;
        float halfVertical = verticalAngle * 0.5f;

        Vector3 origin = pursuerEyes.position;

        Quaternion topLeftRot =
            Quaternion.AngleAxis(-halfHorizontal, pursuerEyes.up) *
            Quaternion.AngleAxis(-halfVertical, pursuerEyes.right);

        Quaternion topRightRot =
            Quaternion.AngleAxis(halfHorizontal, pursuerEyes.up) *
            Quaternion.AngleAxis(-halfVertical, pursuerEyes.right);

        Quaternion bottomLeftRot =
            Quaternion.AngleAxis(-halfHorizontal, pursuerEyes.up) *
            Quaternion.AngleAxis(halfVertical, pursuerEyes.right);

        Quaternion bottomRightRot =
            Quaternion.AngleAxis(halfHorizontal, pursuerEyes.up) *
            Quaternion.AngleAxis(halfVertical, pursuerEyes.right);

        Vector3 topLeft = topLeftRot * pursuerEyes.forward;
        Vector3 topRight = topRightRot * pursuerEyes.forward;
        Vector3 bottomLeft = bottomLeftRot * pursuerEyes.forward;
        Vector3 bottomRight = bottomRightRot * pursuerEyes.forward;

        Gizmos.DrawRay(origin, topLeft * playerDetectionRange);
        Gizmos.DrawRay(origin, topRight * playerDetectionRange);
        Gizmos.DrawRay(origin, bottomLeft * playerDetectionRange);
        Gizmos.DrawRay(origin, bottomRight * playerDetectionRange);

        Gizmos.DrawLine(origin + topLeft * playerDetectionRange, origin + topRight * playerDetectionRange);
        Gizmos.DrawLine(origin + topRight * playerDetectionRange, origin + bottomRight * playerDetectionRange);
        Gizmos.DrawLine(origin + bottomRight * playerDetectionRange, origin + bottomLeft * playerDetectionRange);
        Gizmos.DrawLine(origin + bottomLeft * playerDetectionRange, origin + topLeft * playerDetectionRange);
    }

    public void LookAtPlayer()
    {
        Vector3 playerHead =
           playerDetectionCollider.bounds.center +
           Vector3.up * playerDetectionCollider.bounds.extents.y;

        Vector3 direction = (playerHead - pursuerEyes.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        pursuerEyes.rotation = Quaternion.Slerp(pursuerEyes.rotation, targetRotation, Time.deltaTime * eyesRotationSpeed);
    }

    public void ResetLook() => pursuerEyes.rotation = transform.rotation;

    public void UpdateDestination(Vector3 newDestination) => agent.destination = newDestination;

    public void StopMovement() => agent.isStopped = true;
    public void ResumeMovement() => agent.isStopped = false;
    public void PlayScreechSound() => AudioManager.Instance.PlaySFX(screechName, audioSource);
}
