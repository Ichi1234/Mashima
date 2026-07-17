using UnityEngine;

public class VRHand : MonoBehaviour
{
    [SerializeField] private float handPushForce = 5f;
    private bool isTouchingDoor;

    private void OnCollisionEnter(Collision collision)
    {
        if (isTouchingDoor) return; // already pushed this contact, don't stack
        isTouchingDoor = true;

        Rigidbody body = collision.rigidbody;
        if (body == null || body.isKinematic) return;

        Vector3 pushDir = collision.contacts[0].point - transform.position;
        pushDir.y = 0;
        body.AddForceAtPosition(pushDir.normalized * handPushForce, collision.contacts[0].point);
    }

    private void OnCollisionExit(Collision collision)
    {
        isTouchingDoor = false; // allow next distinct touch to push again
    }
}
