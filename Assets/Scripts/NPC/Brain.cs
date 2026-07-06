using UnityEngine;

public class Brain : MonoBehaviour, INPCInteractable
{
    [SerializeField] private float speedX = 1f;
    [SerializeField] private float speedY = 1.3f;
    [SerializeField] private float magnitudeX = 0.1f;
    [SerializeField] private float magnitudeY = 0.15f;

    private Vector3 startPos;

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float offsetX = Mathf.Sin(Time.time * speedX) * magnitudeX;
        float offsetY = Mathf.Sin(Time.time * speedY) * magnitudeY;

        transform.position = startPos + transform.right * offsetX + transform.up * offsetY;
    }

   

    
}