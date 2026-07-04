using UnityEngine;

public class Door : MonoBehaviour, IInteractable, IImpactReceiver
{
    [Header("Hinge Setup")]
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private float motorForce = 100f;
    [SerializeField] private float openThreshold = 10f;
    [SerializeField] private float angleTolerance = 2f;
    [SerializeField] private float fullyOpenAngle = 90f;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource loopSource;  
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private string creakSfxName = "door-creak";
    [SerializeField] private string slamSfxName = "door-slam";
    [SerializeField] private string closeSfxName = "door-close";

    [Header("Creak Thresholds")]
    [SerializeField] private float creakMoveThreshold = 5f;
    [SerializeField] private float creakStopGrace = 0.4f;

    [Header("Slam Cooldown")]
    [SerializeField] private float slamCooldown = 1f;

    private bool motorActive;
    private float targetAngle;
    private float lastAngle;
    private bool isCreaking = false;
    private bool wasOpen = false;
    private float lastMovingTime = -10f;
    private float lastSlamTime = -10f;

    public bool IsOpen => Mathf.Abs(hinge.angle) > openThreshold;
    public bool IsPartiallyOpen => Mathf.Abs(hinge.angle) > openThreshold
        && Mathf.Abs(hinge.angle) < (fullyOpenAngle - angleTolerance);
    private bool InSlamCooldown => Time.time - lastSlamTime < slamCooldown;

    private void Start()
    {
        lastAngle = hinge.angle;
    }

    public void Interact()
    {
        bool currentlyOpen = Mathf.Abs(hinge.angle) > openThreshold;
        targetAngle = currentlyOpen ? 0f : 90f;

        JointMotor motor = hinge.motor;
        motor.targetVelocity = currentlyOpen ? -motorForce : motorForce;
        motor.force = motorForce;
        hinge.motor = motor;
        hinge.useMotor = true;
        motorActive = true;
    }

    public void OpenWithForce(float force)
    {
        targetAngle = 90f;
        JointMotor motor = hinge.motor;
        motor.targetVelocity = force;
        motor.force = force;
        hinge.motor = motor;
        hinge.useMotor = true;
        motorActive = true;

        PlaySlam();
    }

    public void ReceiveImpact(bool wasRunning)
    {
        if (wasRunning && !InSlamCooldown)
        {
            PlaySlam();
        }
    }

    private void PlaySlam()
    {
        AudioManager.Instance.PlaySFX(slamSfxName, sfxSource);
        lastSlamTime = Time.time;

        if (isCreaking)
        {
            AudioManager.Instance.StopSFX(loopSource);
            isCreaking = false;
        }
    }

    private void Update()
    {
        HandleCloseSound();
    }

    private void FixedUpdate()
    {
        if (motorActive && Mathf.Abs(hinge.angle - targetAngle) < angleTolerance)
        {
            hinge.useMotor = false;
            motorActive = false;
        }

        float angularSpeed = Mathf.Abs(hinge.angle - lastAngle) / Time.fixedDeltaTime;
        lastAngle = hinge.angle;

        HandleCreak(angularSpeed);
    }

    private void HandleCreak(float angularSpeed)
    {
        
        if (InSlamCooldown)
        {
            if (isCreaking)
            {
                AudioManager.Instance.StopSFX(loopSource);
                isCreaking = false;
            }
            return;
        }

        bool isMoving = angularSpeed > creakMoveThreshold;

        if (isMoving)
        {
            lastMovingTime = Time.time;
            if (!isCreaking)
            {
                AudioManager.Instance.PlaySFXLoop(creakSfxName, loopSource);
                isCreaking = true;
            }
        }
        else if (isCreaking && Time.time - lastMovingTime > creakStopGrace)
        {
            AudioManager.Instance.StopSFX(loopSource);
            isCreaking = false;
        }
    }

    private void HandleCloseSound()
    {
        bool currentlyOpen = Mathf.Abs(hinge.angle) > openThreshold;

        if (!currentlyOpen && wasOpen)
        {
            AudioManager.Instance.PlaySFX(closeSfxName, sfxSource);
        }

        wasOpen = currentlyOpen;
    }
}