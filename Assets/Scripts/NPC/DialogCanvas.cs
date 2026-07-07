using System.Collections;
using TMPro;
using UnityEngine;

public class DialogCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI message;

    public Coroutine TypeWriterCo { get; private set; }

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
        if (TypeWriterCo != null)
        {
            StopCoroutine(TypeWriterCo);
        }

        StartCoroutine(TypeWriterTextCO());
    }

    private IEnumerator TypeWriterTextCO()
    {
        int target = message.text.Length;
        int current = 0;

        float timer = 0f;
        float charRate = 0.03f;

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
        TypeWriterCo = null;
    }
    public void SetNpcName(string name) => npcName.text = name;

    public void SetMsg(string newMsg) => message.text = newMsg;
}
