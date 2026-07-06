using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private Canvas dialogCanvas;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private float textDuration = 20;
    private Coroutine typeWriterCo;

    [SerializeField] private List<SpeechDataSO> msgLists;

    public DialogManager Instance { get; private set; }

    private void Awake()
    {
        dialogCanvas.gameObject.SetActive(false);

        Instance = this;
    }

    private void OnEnable()
    {
        TypeWriterTextAnim();
    }

    private void OnDisable()
    {
        message.maxVisibleCharacters = 0;
    }

    private void TypeWriterTextAnim()
    {
        if (typeWriterCo != null)
        {
            StopCoroutine(typeWriterCo);
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
        typeWriterCo = null;
    }

    public void SetNpcName(string name) => npcName.text = name;

    public void SetMsg(string newMsg) => message.text = newMsg;
}
