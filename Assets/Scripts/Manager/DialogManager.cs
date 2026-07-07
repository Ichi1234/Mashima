using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogCanvas dialogCanvas;
    private List<SpeechDataSO> msgLists;
    private Player player;
    private int index = 0;

    public static DialogManager Instance { get; private set; }

    public Action<AudioClip> OnNextMsg;
    public Action OnFinishedTalking;

    private void Awake()
    {
        dialogCanvas.gameObject.SetActive(false);

        Instance = this;

    }


    private void Update()
    {
        if (player != null &&
            player.Input.Player.NPCInteraction.WasPerformedThisFrame()
            && dialogCanvas.isActiveAndEnabled)
        {
            NextMsg();
        }
    }

    public void InitializePlayer(Player player) => this.player = player;

    public void NextMsg()
    {
        if (!dialogCanvas.IsTypingAnimFinished)
        {
            dialogCanvas.SkipAnimation();
            return;
        }

        index++;

        if (index >= msgLists.Count)
        {
            OnFinishedTalking?.Invoke();
            CloseDialogBox();
            return;
        }


        OnNextMsg?.Invoke(msgLists[index].speechSound);
        dialogCanvas.SetMsg(msgLists[index].speechText);
        dialogCanvas.PlayTypeWriterTextAnimation();
    }

    public void OpenDialogBox(string npcName, List<SpeechDataSO> msgData)
    {
        if (dialogCanvas.isActiveAndEnabled) return;

        msgLists = msgData;

        dialogCanvas.SetNpcName(npcName);
        dialogCanvas.SetMsg(msgLists[index].speechText);
        dialogCanvas.gameObject.SetActive(true);
    }

    public void CloseDialogBox()
    {
        index = 0;
        dialogCanvas.gameObject.SetActive(false);
    }
}
