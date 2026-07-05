using UnityEngine;

public class Brain : MonoBehaviour
{
    [SerializeField] private float speedX = 1f;
    [SerializeField] private float speedY = 1.3f;
    [SerializeField] private float magnitudeX = 0.1f;
    [SerializeField] private float magnitudeY = 0.15f;

    [SerializeField] private float rotationSpeed = 5f;

    private Vector3 startPos;
    private Quaternion startRotation;

    private Transform target;

    private void Start()
    {
        startPos = transform.position;
        startRotation = transform.rotation;
    }

    private void Update()
    {
        float offsetX = Mathf.Sin(Time.time * speedX) * magnitudeX;
        float offsetY = Mathf.Sin(Time.time * speedY) * magnitudeY;

        transform.position = startPos + transform.right * offsetX + transform.up * offsetY;
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        if (target != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                startRotation,
                rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            target = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            target = null;
    }
}