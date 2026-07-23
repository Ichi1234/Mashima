using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ResearchLogger.Log("Game ended");
        GameManager.Instance.SetGameEnd();
    }
}
