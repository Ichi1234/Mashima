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
    public Action OnNpcInteract;
    public Action OnFinishedTalking;

    private void Awake()
    {
        dialogCanvas.gameObject.SetActive(false);

        Instance = this;

    }

    private void OnEnable() => OnNpcInteract += NextMsg;

    private void Update()
    {
        if (player != null &&
            player.Input.Player.NPCInteraction.WasPerformedThisFrame()
            && dialogCanvas.isActiveAndEnabled)
        {
            NextMsg();
        }
    }

    private void OnDisable()
    {
        OnNpcInteract -= NextMsg;
    }

    public void InitializePlayer(Player player) => this.player = player;

    public void NextMsg()
    {
        index++;

        if (index >= msgLists.Count)
        {
            OnFinishedTalking?.Invoke();
            CloseDialogBox();
            return;
        }

        dialogCanvas.SetMsg(msgLists[index].speechText);
        dialogCanvas.PlayTypeWriterTextAnimation();
    }

    public void OpenDialogBox(string npcName, List<SpeechDataSO> msgData)
    {
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
