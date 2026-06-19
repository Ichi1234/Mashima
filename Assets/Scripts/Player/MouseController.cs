using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform playerCameraOffset;
    [SerializeField] private float mouseSensitivity = 1;

    private float pitch = 0;
    private float yaw = 0;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pitch = 0;
        playerCameraOffset.localRotation = Quaternion.Euler(0, 0, 0);

    }

    private void Update()
    {
        if (player.Input.UI.Menu.WasPerformedThisFrame())
        {
            ToggleMouseCursor();
        }

        RotationByMouse();
    }

    private void ToggleMouseCursor()
    {
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void RotationByMouse()
    {
        Vector2 mouseDir = player.Input.Player.Mouse.ReadValue<Vector2>();

        yaw += mouseDir.x * mouseSensitivity;
        pitch += -mouseDir.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -75, 75);

        playerCameraOffset.localRotation = Quaternion.Euler(pitch, 0, 0);
        player.transform.localRotation = Quaternion.Euler(0, yaw, 0);
    }
}
