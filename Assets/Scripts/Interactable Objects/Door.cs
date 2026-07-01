using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private float motorForce = 100f;
    [SerializeField] private float openThreshold = 10f;
    [SerializeField] private float angleTolerance = 2f;

    private float targetAngle;
    private bool motorActive;

    public bool IsOpen => Mathf.Abs(hinge.angle) > openThreshold;

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

    private void Update()
    {
        if (!motorActive) return;

        if (Mathf.Abs(hinge.angle - targetAngle) < angleTolerance)
        {
            hinge.useMotor = false;
            motorActive = false;
        }
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
    }
}