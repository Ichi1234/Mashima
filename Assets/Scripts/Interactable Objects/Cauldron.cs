using UnityEngine;

public class Cauldron : MonoBehaviour, IPuzzleReactable
{
    public void OnItemDeposited(GameObject itemPrefab)
    {
        Vector3 dropPoint = new Vector3(
            transform.position.x,
            transform.position.y + 0.9f,
            transform.position.z
        );

        Instantiate(itemPrefab, dropPoint, Quaternion.identity);
    }
}
