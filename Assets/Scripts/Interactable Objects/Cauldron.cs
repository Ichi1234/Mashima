using UnityEngine;

public class Cauldron : MonoBehaviour, IPuzzleReactable
{
    public void OnItemDeposited(GameObject itemPrefab)
    {
        if (GameManager.Instance.CurPlayerMode == PlayerMode.VR)
        {
            return;
        }

        Vector3 dropPoint = new Vector3(
            transform.position.x,
            transform.position.y + 0.9f,
            transform.position.z
        );

        Item item = itemPrefab.GetComponent<Item>();
        item.DisableInteract();

        Instantiate(item, dropPoint, Quaternion.identity);
    }

    public void OnPuzzleCompleted() => GameManager.Instance.OnElectricRepaired?.Invoke();
}
