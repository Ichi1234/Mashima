using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    private Camera mainCamera;

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

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (player == null || !sprite.enabled)
            return;

        float distance =
            Vector3.Distance(player.position, transform.position);

    }

    private void LateUpdate()
    {
        if (mainCamera == null || !sprite.enabled)
            return;

        transform.forward = mainCamera.transform.forward;
    }
}