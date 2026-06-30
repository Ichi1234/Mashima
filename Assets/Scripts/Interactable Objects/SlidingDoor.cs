using System.Collections;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private float slideDuration = 1f;
    [SerializeField] private string openValueParam = "openValue";

    [SerializeField] private bool hasElectricity = false;
    private Coroutine slideCoroutine;
    private bool isOpen = false;

    private void Awake()
    {
        doorAnimator.SetFloat(openValueParam, 0f);
    }

    private void Update()
    {
        doorAnimator.GetFloat(openValueParam);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !hasElectricity) return;

        if (!isOpen)
        {
            isOpen = true;
            SetDoorOpen(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !hasElectricity) return;

        if (isOpen)
        {
            isOpen = false;
            SetDoorOpen(false);
        }
    }

    [ContextMenu("Test Open")]
    private void TestOpen() => SetDoorOpen(true);

    [ContextMenu("Test Close")]
    private void TestClose() => SetDoorOpen(false);

    private void SetDoorOpen(bool open)
    {
        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        slideCoroutine = StartCoroutine(BlendDoor(open ? 1f : 0f));
    }

    private IEnumerator BlendDoor(float target)
    {
        float start = doorAnimator.GetFloat(openValueParam);
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            doorAnimator.SetFloat(openValueParam, Mathf.Lerp(start, target, t));
            yield return null;
        }

        doorAnimator.SetFloat(openValueParam, target);
    }
}