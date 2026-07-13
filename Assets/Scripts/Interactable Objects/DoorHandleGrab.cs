using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class DoorHandleGrab : MonoBehaviour
{
    [SerializeField] private HingeJoint doorHinge;
    [SerializeField] private Transform hingeAxisTransform; // pivot point, on the door's hinge side
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float motorForce = 5000f;

    private XRGrabInteractable grabInteractable;
    private IXRSelectInteractor currentInteractor;

    private float grabStartHandAngle;
    private float grabStartDoorAngle;
    private bool isGrabbed;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // Handle shouldn't physically fly off on its own - it's just a grab target
        if (TryGetComponent(out Rigidbody handleRb))
            handleRb.isKinematic = true;

        grabInteractable.trackPosition = false; // don't let XRI move this transform directly
        grabInteractable.trackRotation = false;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        currentInteractor = args.interactorObject;
        isGrabbed = true;

        Vector3 hingeToHand = currentInteractor.transform.position - hingeAxisTransform.position;
        hingeToHand.y = 0f;
        grabStartHandAngle = Mathf.Atan2(hingeToHand.x, hingeToHand.z) * Mathf.Rad2Deg;
        grabStartDoorAngle = doorHinge.angle;
    }

    private void OnRelease(SelectExitEventArgs args) => (currentInteractor, isGrabbed) = (null, false);

    void FixedUpdate()
    {
        if (!isGrabbed || currentInteractor == null) return;

        Vector3 hingeToHand = currentInteractor.transform.position - hingeAxisTransform.position;
        hingeToHand.y = 0f;
        if (hingeToHand.sqrMagnitude < 0.0001f) return;

        float currentHandAngle = Mathf.Atan2(hingeToHand.x, hingeToHand.z) * Mathf.Rad2Deg;
        float handAngleDelta = Mathf.DeltaAngle(grabStartHandAngle, currentHandAngle);

        float targetAngle = grabStartDoorAngle + handAngleDelta; // door moves BY however much hand moved

        JointSpring spring = doorHinge.spring;
        spring.spring = 200f;
        spring.damper = 20f;
        spring.targetPosition = targetAngle;
        doorHinge.spring = spring;
        doorHinge.useSpring = true;
        doorHinge.useMotor = false;
    }

}