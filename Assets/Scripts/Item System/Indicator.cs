using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    private Transform player;

    public void RecivedPlayerData(Transform player)
    {
        this.player = player;
    }

    public void Show()
    {
        sprite.enabled = true;
    }

    public void Hide()
    {
        sprite.enabled = false;
    }

    private void Update()
    {
        if (player == null || !sprite.enabled)
            return;

        float distance =
            Vector3.Distance(player.position, transform.position);

    }
}