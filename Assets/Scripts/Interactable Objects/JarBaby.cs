using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class JarBaby : MonoBehaviour, IPuzzleReactable
{
    [SerializeField] private GameObject jarGlass;
    [SerializeField] private Rigidbody baby;
    [SerializeField] private Rigidbody maskRB;
    [SerializeField] private Item mask;
    [SerializeField] private Collider maskCollider;
    [SerializeField] private XRGrabInteractable xRGrab;

    private void Awake()
    {
        mask.DisableInteract();
        maskCollider.enabled = false;    
    }

    public void OnItemDeposited(GameObject itemPrefab)
    {
        return;
    }

    public void OnPuzzleCompleted()
    {
        jarGlass.SetActive(false);

        baby.isKinematic = false;
        baby.useGravity = true;

        mask.EnableInteract();
        maskCollider.enabled = true;
        maskRB.isKinematic = false;
        maskRB.useGravity = true;

        xRGrab.enabled = true;
    }
}
