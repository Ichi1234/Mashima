using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private EyeVfx eyeVfx;
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private float fadingTextDuration;
    private float maxTextAlpha = 255;

    [ContextMenu("PlayWakeupEffect")]
    public void PlayWakeUpEffect()
    {
        eyeVfx.gameObject.SetActive(true);
        eyeVfx.PlayWakeupEffect();
    }

    [ContextMenu("PlayEndingEffect")]
    public void PlayEndingScene()
    {
        StopAllCoroutines();

        StartCoroutine(PlayEndingSceneCo());
    }

    private IEnumerator PlayEndingSceneCo()
    {
        eyeVfx.gameObject.SetActive(true);
        endingText.gameObject.SetActive(true);

        yield return StartCoroutine(eyeVfx.CloseEyeCo());
        yield return StartCoroutine(SlowyIncreaseTextAlpha());
    }

    private IEnumerator SlowyIncreaseTextAlpha()
    {
        float elapsed = 0;
        while (elapsed <= fadingTextDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadingTextDuration;

            endingText.alpha = Mathf.Lerp(0, maxTextAlpha, t);
            yield return null;
        }

        endingText.alpha = maxTextAlpha;
    }
}
