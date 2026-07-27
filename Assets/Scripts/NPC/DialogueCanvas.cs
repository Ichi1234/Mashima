using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI message;

    private Coroutine typeWriterCo;
    public bool IsTypingAnimFinished { get; private set; }

    private void OnEnable()
    {
        PlayTypeWriterTextAnimation();
    }

    private void OnDisable()
    {
        message.maxVisibleCharacters = 0;
    }

    public void PlayTypeWriterTextAnimation()
    {
        if (typeWriterCo != null)
        {
            StopCoroutine(typeWriterCo);
        }

        typeWriterCo = StartCoroutine(TypeWriterTextCO());
    }

    private IEnumerator TypeWriterTextCO()
    {
        int target = message.text.Length;
        int current = 0;

        float timer = 0f;
        float charRate = 0.03f;

        message.maxVisibleCharacters = current;

        IsTypingAnimFinished = false;

        while (current < target)
        {
            timer += Time.deltaTime;

            if (timer >= charRate)
            {
                timer -= charRate;
                current++;
                message.maxVisibleCharacters = current;
            }

            yield return null;
        }

        message.maxVisibleCharacters = message.text.Length;
        IsTypingAnimFinished = true;
        typeWriterCo = null;
    }
    public void SetNpcName(string name) => npcName.text = name;

    public void SetMsg(string newMsg) => message.text = newMsg;

    public void SkipAnimation()
    {
        if (typeWriterCo != null)
        {
            StopCoroutine(typeWriterCo);
        }

        IsTypingAnimFinished = true;

        message.maxVisibleCharacters = message.text.Length;
    }
}
