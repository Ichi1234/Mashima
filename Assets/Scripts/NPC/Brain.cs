using System;
using System.Collections.Generic;
using UnityEngine;

public enum NPCStage { FirstMeet, TalkSecondTime, PuzzleFinished }
public enum NPCID { Brian }

public class Brain : MonoBehaviour, INPCInteractable
{
    [SerializeField] private NPCID npcID;
    [SerializeField] private string npcName;

    [SerializeField] private float speedX = 1f;
    [SerializeField] private float speedY = 1.3f;
    [SerializeField] private float magnitudeX = 0.1f;
    [SerializeField] private float magnitudeY = 0.15f;

    [SerializeField] private List<SpeechGroup> msgList;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private DialogueCanvas vrDialogueBox;
    [SerializeField] private Indicator interactionIndicator;

    private float talkCooldownDuration = 0.5f;
    private float lastTalkedTime = 0;

    private bool isInteractable = true;

    private bool isFinishedTalking = false;

    private Vector3 startPos;
    
    private NPCStage currentStage = NPCStage.FirstMeet;

    private NPCStage LastStage =>
    (NPCStage)(Enum.GetValues(typeof(NPCStage)).Length - 1);


    public void Interact()
    {
        if (!isInteractable) return;
        Speak();
    }

    private void Speak()
    {
        isInteractable = false;
        isFinishedTalking = false;

        DialogManager.Instance.OnFinishedTalking += FinishedTalking;
        DialogManager.Instance.OnNextMsg += PlaySound;

        SpeechGroup speechGroup = GetSpeechGroup();

        interactionIndicator.SetShowable(false);

        if (GameManager.Instance.IsInVR) DialogManager.Instance.SetCanvas(vrDialogueBox);
        DialogManager.Instance.OpenDialogBox(npcID, npcName, currentStage, speechGroup.SpeechList);
        PlaySound(speechGroup.SpeechList[0].speechSound);
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void OnEnable()
    {
        PuzzleManager.Instance.OnPuzzleStateChanged += HandlePuzzleStateChanged;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameEnded) return;

        float offsetX = Mathf.Sin(Time.time * speedX) * magnitudeX;
        float offsetY = Mathf.Sin(Time.time * speedY) * magnitudeY;

        transform.position = startPos + transform.right * offsetX + transform.up * offsetY;

        if (isFinishedTalking && Time.time - lastTalkedTime > talkCooldownDuration)
        {
            isInteractable = true;
        }
    }

    public void HandlePuzzleStateChanged(PuzzleID puzzleID, PuzzleState puzzleState)
    {
        if (puzzleID != PuzzleID.Cauldron) return;
        if (puzzleState != PuzzleState.Completed) return;

        currentStage = NPCStage.PuzzleFinished;

        Speak();
    }

    private void FinishedTalking(NPCID npcName, NPCStage npcStage)
    {
        if (npcName != this.npcID) return;

        if (currentStage < LastStage && !GetSpeechGroup().loop)
        {
            currentStage++;
        }
        else if (currentStage >= LastStage && !GetSpeechGroup().loop)
        {
            isInteractable = false;
        }

        DialogManager.Instance.OnFinishedTalking -= FinishedTalking;
        DialogManager.Instance.OnNextMsg -= PlaySound;

        interactionIndicator.SetShowable(true);
        lastTalkedTime = Time.time;
        isFinishedTalking = true;
    }

    private SpeechGroup GetSpeechGroup() => msgList[(int)currentStage];

    public void StopSpeechGroupLoop()
    {
        msgList[(int)currentStage].loop = false;
        currentStage++;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (clip == null) return;
       
        audioSource.clip = clip;
        audioSource.Play();
        
    }
}


