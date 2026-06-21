using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WakeUpVfx : MonoBehaviour
{
    [SerializeField] private RectTransform eyeTop;
    [SerializeField] private RectTransform eyeBottom;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float closeEyeDuration = 0.5f;
    [SerializeField] private float openBrieflyDuration = 0.25f;
    [SerializeField] private Volume blurEffect;
    [SerializeField] private float blurDuration = 1f;
    [SerializeField] private float targetFocusDistance = 50f;

    private DepthOfField depthOfField;



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

        blurEffect.profile.TryGet(out depthOfField);
    }

    private void Update()
    {
        eyeTop.anchoredPosition = new Vector3(0, curTopEyePos, 0);
        eyeBottom.anchoredPosition = new Vector3(0, curBottomEyePos, 0);

        if (curTopEyePos == defaultTopPos.y)
        {
            //blurEffect.weight = 0;
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

    private IEnumerator BlurFocusCo()
    {
        float startValue = 0.1f;
        float elapsed = 0f;

        while (elapsed < blurDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / blurDuration;
            depthOfField.focusDistance.value = Mathf.Lerp(startValue, targetFocusDistance, t);
            yield return null;
        }

        depthOfField.focusDistance.value = targetFocusDistance;
        blurEffect.weight = 0;

    }

    private IEnumerator PlayWakeUpAnimationCo()
    {
        depthOfField.focusDistance.value = 0.1f;
        blurEffect.weight = 1;

        yield return StartCoroutine(BrieftlyOpenEyeCo());
        yield return new WaitForSeconds(openBrieflyDuration);
        yield return StartCoroutine(CloseEyeCo());
        yield return new WaitForSeconds(openBrieflyDuration);
        yield return StartCoroutine(BrieftlyOpenEyeCo());
        yield return new WaitForSeconds(openBrieflyDuration);
        yield return StartCoroutine(CloseEyeCo());
        yield return new WaitForSeconds(closeEyeDuration);
        yield return StartCoroutine(OpenEyeCo());
        yield return StartCoroutine(BlurFocusCo()); 

        blurEffect.weight = 0;

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
