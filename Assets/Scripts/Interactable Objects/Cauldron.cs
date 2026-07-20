using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour, IPuzzleReactable
{
    [SerializeField] private ParticleSystem doneEffect;

    private List<GameObject> itemLists = new List<GameObject>();

    public void OnItemDeposited(GameObject itemPrefab)
    {
        if (GameManager.Instance.IsInVR)
        {
            itemLists.Add(itemPrefab);
            return;
        }

        Vector3 dropPoint = new Vector3(
            transform.position.x,
            transform.position.y + 0.9f,
            transform.position.z
        );

        Item item = itemPrefab.GetComponent<Item>();

        Item newItem = Instantiate(item, dropPoint, Quaternion.identity);

        itemLists.Add(newItem.gameObject);
    }

    public void OnPuzzleCompleted()
    {
        foreach (var item in itemLists)
        {
            Destroy(item);    
        }

        doneEffect.gameObject.SetActive(true);
        GameManager.Instance.OnElectricRepaired?.Invoke();
        
    }
}
