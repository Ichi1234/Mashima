using System.Collections;
using UnityEngine;

public class WakeUpVfx : MonoBehaviour
{
    [SerializeField] private RectTransform eyeTop;
    [SerializeField] private RectTransform eyeBottom;
    [SerializeField] private RectTransform blurEffect;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float closeEyeDuration = 0.5f;
    [SerializeField] private float openBrieflyDuration = 0.25f;

    private Coroutine deathCo;

    private Vector3 defaultTopPos;
    private Vector3 defaultBottomPos;

    private float curTopEyePos;
    private float curBottomEyePos;

    private void Awake()
    {
        defaultTopPos = eyeTop.anchoredPosition;
        defaultBottomPos = eyeBottom.anchoredPosition;

        curTopEyePos = defaultTopPos.y;
        curBottomEyePos = defaultBottomPos.y;

    }

    private void Update()
    {
        eyeTop.anchoredPosition = new Vector3(0, curTopEyePos, 0);
        eyeBottom.anchoredPosition = new Vector3(0, curBottomEyePos, 0);

        if (curTopEyePos == defaultTopPos.y)
        {
            blurEffect.gameObject.SetActive(false);
        }
    }

    [ContextMenu("PlayDeathEffect")]
    public void Play()
    {
        if (deathCo != null)
        {
            StopCoroutine(deathCo);
        }

        curTopEyePos = 0;
        curBottomEyePos = 0;

        deathCo = StartCoroutine(PlayWakeUpAnimationCo());
    }

    private IEnumerator PlayWakeUpAnimationCo()
    {
        blurEffect.gameObject.SetActive(true);

        yield return StartCoroutine(BrieftlyOpenEyeCo());
        yield return new WaitForSeconds(openBrieflyDuration);
        yield return StartCoroutine(CloseEyeCo());
        yield return new WaitForSeconds(openBrieflyDuration);
        yield return StartCoroutine(BrieftlyOpenEyeCo());
        yield return new WaitForSeconds(openBrieflyDuration);
        yield return StartCoroutine(CloseEyeCo());
        yield return new WaitForSeconds(closeEyeDuration);
        yield return StartCoroutine(OpenEyeCo());
    }

    private IEnumerator EyeAnimation(float targetTopPos, float targetBottomPos)
    {
        float originalTopEyePos = curTopEyePos;
        float originalBottomEyePos = curBottomEyePos;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            float eased = t * t * t;

            curTopEyePos = Mathf.Lerp(originalTopEyePos, targetTopPos, eased);
            curBottomEyePos = Mathf.Lerp(originalBottomEyePos, targetBottomPos, eased);

            yield return null;
        }

        curTopEyePos = targetTopPos;
        curBottomEyePos = targetBottomPos;
    }

    private IEnumerator CloseEyeCo()
    {
        yield return EyeAnimation(0, 0);

    }

    private IEnumerator BrieftlyOpenEyeCo()
    {
        yield return EyeAnimation(defaultTopPos.y / 2, defaultBottomPos.y / 2);

    }
    private IEnumerator OpenEyeCo()
    {
        yield return EyeAnimation(defaultTopPos.y, defaultBottomPos.y);
    }
}
