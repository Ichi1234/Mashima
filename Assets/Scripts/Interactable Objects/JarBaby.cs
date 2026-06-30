using UnityEngine;
using UnityEngine.UI;

public class JarBaby : MonoBehaviour, IPuzzleReactable
{
    [SerializeField] private GameObject jarGlass;
    [SerializeField] private Rigidbody baby;
    [SerializeField] private Rigidbody maskRB;
    [SerializeField] private Item mask;
    [SerializeField] private Rigidbody tearBottle;

    private void Awake()
    {
        mask.DisableInteract();
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
        maskRB.isKinematic = false;
        maskRB.useGravity = true;
    }
}
