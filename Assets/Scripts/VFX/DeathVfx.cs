using System.Collections;
using UnityEngine;

public class DeathVfx : MonoBehaviour
{
    [SerializeField] private RectTransform eyeTop;
    [SerializeField] private RectTransform eyeBottom;
    [SerializeField] private RectTransform blurEffect;
    [SerializeField] private float animationDuration = 2.5f;
    [SerializeField] private float closeEyeDuration = 0.5f;

    private Coroutine deathCo;

    private Vector3 defaultTopPos;
    private Vector3 defaultBottomPos;

    private float curTopEyePos;
    private float curBottomEyePos;

    private void Awake()
    {
        defaultTopPos = eyeTop.anchoredPosition;
        defaultBottomPos = eyeBottom.anchoredPosition;
    }

    private void Start()
    {
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

        deathCo = StartCoroutine(PlayBlinkAnimationCo());

        
    }

    private IEnumerator PlayBlinkAnimationCo()
    {
        blurEffect.gameObject.SetActive(true);
        yield return StartCoroutine(CloseEyeCo());
        yield return new WaitForSeconds(closeEyeDuration);
        yield return StartCoroutine(OpenEyeCo());
    }

    private IEnumerator CloseEyeCo()
    {
        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / closeEyeDuration; 
            float eased = t * t * t; 

            curTopEyePos = Mathf.Lerp(defaultTopPos.y, 0, eased);
            curBottomEyePos = Mathf.Lerp(defaultBottomPos.y, 0, eased);

            yield return null; 
        }

        curTopEyePos = 0;
        curBottomEyePos = 0;
    }

    private IEnumerator OpenEyeCo()
    {
        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / closeEyeDuration; 
            float eased = t * t * t; 

            curTopEyePos = Mathf.Lerp(0, defaultTopPos.y, eased);
            curBottomEyePos = Mathf.Lerp(0, defaultBottomPos.y, eased);

            yield return null; 
        }

        curTopEyePos = defaultTopPos.y;
        curBottomEyePos = defaultBottomPos.y;
    }
}
