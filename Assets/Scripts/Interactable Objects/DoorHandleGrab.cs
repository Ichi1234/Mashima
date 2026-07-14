using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class DoorHandleGrab : MonoBehaviour
{
    [SerializeField] private HingeJoint doorHinge;
    [SerializeField] private Rigidbody doorRb;
    [SerializeField] private Transform hingeAxisTransform;
    [SerializeField] private Transform doorTransform; // NEW: assign the door's transform directly
    [SerializeField] private float maxGrabDistance = 0.5f;
    [SerializeField] private Door door;


    private XRGrabInteractable grabInteractable;
    private IXRSelectInteractor currentInteractor;
    private float grabStartHandAngle;
    private float grabStartDoorAngle;
    private float previousTargetAngle;
    private float targetAngleLastFrame;

    private bool isGrabbed;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        if (TryGetComponent(out Rigidbody handleRb))
            handleRb.isKinematic = true;

        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;

        // Cache handle's offset relative to the door, while still normally parented
        localOffsetPos = doorTransform.InverseTransformPoint(transform.position);
        localOffsetRot = Quaternion.Inverse(doorTransform.rotation) * transform.rotation;
    }

    private Vector3 localOffsetPos;
    private Quaternion localOffsetRot;

    void LateUpdate()
    {
        // Force the handle to stick to the door's current rotation,
        // regardless of whatever XRI did to its actual Transform parent
        transform.position = doorTransform.TransformPoint(localOffsetPos);
        transform.rotation = doorTransform.rotation * localOffsetRot;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        currentInteractor = args.interactorObject;
        isGrabbed = true;
        door.IsBeingGrabbed = true;

        Vector3 hingeToHand = currentInteractor.transform.position - hingeAxisTransform.position;
        hingeToHand.y = 0f;
        grabStartHandAngle = Mathf.Atan2(hingeToHand.x, hingeToHand.z) * Mathf.Rad2Deg;
        grabStartDoorAngle = doorHinge.angle;
        previousTargetAngle = grabStartDoorAngle;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        currentInteractor = null;
        isGrabbed = false;
        door.IsBeingGrabbed = false;

        // Hand off current swing speed as real physics velocity
        float releaseAngularSpeed = (targetAngleLastFrame - previousTargetAngle) / Time.fixedDeltaTime;
        Vector3 worldAxis = doorTransform.TransformDirection(doorHinge.axis).normalized;
        doorRb.angularVelocity = worldAxis * releaseAngularSpeed * Mathf.Deg2Rad;
    }

    void FixedUpdate()
    {
        if (!isGrabbed || currentInteractor == null) return;

        float handDist = Vector3.Distance(currentInteractor.transform.position, transform.position);
        if (handDist > maxGrabDistance)
        {
            grabInteractable.interactionManager.SelectExit(currentInteractor, grabInteractable);
            return;
        }

        Vector3 hingeToHand = currentInteractor.transform.position - hingeAxisTransform.position;
        hingeToHand.y = 0f;
        if (hingeToHand.sqrMagnitude < 0.0001f) return;

        float currentHandAngle = Mathf.Atan2(hingeToHand.x, hingeToHand.z) * Mathf.Rad2Deg;
        float handAngleDelta = Mathf.DeltaAngle(grabStartHandAngle, currentHandAngle);
        handAngleDelta = -handAngleDelta; // flip if direction still wrong

        float targetAngle = grabStartDoorAngle + handAngleDelta;
        if (doorHinge.useLimits)
            targetAngle = Mathf.Clamp(targetAngle, doorHinge.limits.min, doorHinge.limits.max);

        previousTargetAngle = targetAngleLastFrame;
        targetAngleLastFrame = targetAngle;

        float deltaFromCurrent = targetAngle - doorHinge.angle;
        Vector3 worldAxis = doorTransform.TransformDirection(doorHinge.axis).normalized;

        if (float.IsNaN(deltaFromCurrent) || float.IsNaN(worldAxis.x) || doorRb.rotation.x != doorRb.rotation.x)
        {
            Debug.LogWarning("Door rotation corrupted - resetting.");
            doorRb.rotation = Quaternion.identity;
            doorRb.angularVelocity = Vector3.zero;
            isGrabbed = false; // force-release so it doesn't immediately re-corrupt next frame
            return;
        }
        doorRb.MoveRotation(doorRb.rotation * Quaternion.AngleAxis(deltaFromCurrent, worldAxis));
    }
}