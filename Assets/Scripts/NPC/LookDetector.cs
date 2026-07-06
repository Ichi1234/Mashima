using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LookDetector : MonoBehaviour
{
    [SerializeField] private Transform npcTransform;
    [SerializeField] private float rotationSpeed = 5f;

    private Quaternion startRotation;

    private Transform target;

    private void Start()
    {
        startRotation = npcTransform.rotation;

    }

    private void Update()
    {
        LookAtPlayer();


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

    private void LookAtPlayer()
    {
        if (target != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - npcTransform.position);

            npcTransform.rotation = Quaternion.Slerp(
                npcTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }
        else
        {
            npcTransform.rotation = Quaternion.Slerp(
                npcTransform.rotation,
                startRotation,
                rotationSpeed * Time.deltaTime);
        }
    }
}
