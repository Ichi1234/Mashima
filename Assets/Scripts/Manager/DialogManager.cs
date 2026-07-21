using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogueCanvas desktopDialogCanvas;
    private DialogueCanvas curCanvas;

    private List<SpeechDataSO> msgLists;
    private Player player;
    private int index = 0;

    public static DialogManager Instance { get; private set; }

    public Action<AudioClip> OnNextMsg;
    public Action<NPCID, NPCStage> OnFinishedTalking;

    private NPCID curNpcID;
    private string curNpcName;
    private NPCStage curNpcStage;

    private void Awake()
    {
        desktopDialogCanvas.gameObject.SetActive(false);


        Instance = this;

    }

    private void Start()
    {
        if (!GameManager.Instance.IsInVR) curCanvas = desktopDialogCanvas;
    }


    private void Update()
    {
        if (player != null &&
            curCanvas != null &&
            player.Input.Player.NPCInteraction.WasPerformedThisFrame()
            && curCanvas.isActiveAndEnabled)
        {
            NextMsg();
        }
    }

    public void InitializePlayer(Player player) => this.player = player;

    public void NextMsg()
    {
        if (!curCanvas.IsTypingAnimFinished)
        {
            curCanvas.SkipAnimation();
            return;
        }

        index++;

        if (index >= msgLists.Count)
        {
            OnFinishedTalking?.Invoke(curNpcID, curNpcStage);
            CloseDialogBox();
            return;
        }


        OnNextMsg?.Invoke(msgLists[index].speechSound);
        curCanvas.SetMsg(msgLists[index].speechText);
        curCanvas.PlayTypeWriterTextAnimation();
    }

    public void OpenDialogBox(NPCID npcID, string npcName, NPCStage npcState, List<SpeechDataSO> msgData)
    {
        if (curCanvas.isActiveAndEnabled) return;

        msgLists = msgData;
        curNpcID = npcID;
        curNpcName = npcName;
        curNpcStage = npcState;
        
        curCanvas.SetNpcName(curNpcName);
        curCanvas.SetMsg(msgLists[index].speechText);
        curCanvas.gameObject.SetActive(true);
    }

    public void CloseDialogBox()
    {
        index = 0;
        curCanvas.gameObject.SetActive(false);
    }

    public void SetCanvas(DialogueCanvas newCanvas) => curCanvas = newCanvas;
}
