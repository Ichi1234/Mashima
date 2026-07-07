using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite interactableSprite;
    [SerializeField] private bool isAdjustScale = true;

    public bool IsShowable { get; private set; } = true;

    private Camera mainCamera;

    private Transform player;

    public void RecivedPlayerData(Transform player)
    {
        this.player = player;
    }

    public void Show()
    {
        if (IsShowable)
        {
            sr.enabled = true;
        }
    }

    public void Hide()
    {
        sr.enabled = false;
    }

    private void Awake()
    {
        mainCamera = Camera.main;

        sr.sprite = defaultSprite;
    }

    private void Start()
    {
        if (isAdjustScale)
        {
            Vector3 parentScale = transform.parent.lossyScale;
            transform.localScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z
            );
        }
    }

    private void Update()
    {
        if (sr.enabled == true && !IsShowable) sr.enabled = false;

        if (player == null || !sr.enabled)
            return;
       
    }

    private void LateUpdate()
    {
        if (mainCamera == null || !sr.enabled)
            return;

        transform.forward = mainCamera.transform.forward;
    }

    public void SetInteractable(bool interactable)
    {
        if (sr == null) return;

        sr.sprite = interactable
            ? interactableSprite
            : defaultSprite;
    }

    public void SetShowable(bool isInteractable) => IsShowable = isInteractable;
}
