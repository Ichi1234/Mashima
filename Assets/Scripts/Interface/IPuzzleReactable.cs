using UnityEngine;

public interface IPuzzleReactable
{
    public void OnItemDeposited(GameObject itemPrefab);

    public void OnPuzzleCompleted();
}
