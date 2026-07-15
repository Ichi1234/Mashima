using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class DoorHandleGrab : MonoBehaviour
{
    [SerializeField] private float maxGrabDistance = 0.4f;
    [SerializeField] private Transform handleColliderA;
    [SerializeField] private Transform handleColliderB; 

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        for (int i = grabInteractable.interactorsSelecting.Count - 1; i >= 0; i--)
        {
            IXRSelectInteractor interactor = grabInteractable.interactorsSelecting[i];

            // Check distance against whichever handle collider is actually closer to this hand
            float distA = Vector3.Distance(interactor.transform.position, handleColliderA.position);
            float distB = Vector3.Distance(interactor.transform.position, handleColliderB.position);
            float closestDist = Mathf.Min(distA, distB);

            if (closestDist > maxGrabDistance)
            {
                grabInteractable.interactionManager.SelectExit(interactor, grabInteractable);
            }
        }
    }
}