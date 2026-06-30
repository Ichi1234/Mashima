using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private float motorForce = 50f;
    [SerializeField] private float openThreshold = 10f;
    [SerializeField] private float angleTolerance = 2f;

    private float targetAngle;
    private bool motorActive;

    public void Interact()
    {
        Debug.Log("I am door and I am reacting");

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
}