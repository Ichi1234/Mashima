using System;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour, INPCInteractable
{
    [SerializeField] private float speedX = 1f;
    [SerializeField] private float speedY = 1.3f;
    [SerializeField] private float magnitudeX = 0.1f;
    [SerializeField] private float magnitudeY = 0.15f;

    [SerializeField] private string npcName;
    [SerializeField] private List<SpeechGroup> msgList;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private DialogueCanvas vrDialogueBox;
    [SerializeField] private Indicator interactionIndicator;

    private float talkCooldownDuration = 0.5f;
    private float lastTalkedTime = 0;

    private enum BrianStage { FirstMeet, TalkSecondTime }

    private bool isInteractable = true;

    private bool isFinishedTalking = false;

    private Vector3 startPos;
    
    private BrianStage currentStage = BrianStage.FirstMeet;


    public void Interact()
    {
        if (!isInteractable) return;

        isInteractable = false;
        isFinishedTalking = false;

        DialogManager.Instance.OnFinishedTalking += FinishedTalking;
        DialogManager.Instance.OnNextMsg += PlaySound;

        List<SpeechDataSO> speechList = GetSpeechList();

        interactionIndicator.SetShowable(false);
        DialogManager.Instance.SetCanvas(vrDialogueBox);
        DialogManager.Instance.OpenDialogBox(npcName, speechList);
        PlaySound(speechList[0].speechSound);
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float offsetX = Mathf.Sin(Time.time * speedX) * magnitudeX;
        float offsetY = Mathf.Sin(Time.time * speedY) * magnitudeY;

        transform.position = startPos + transform.right * offsetX + transform.up * offsetY;

        if (isFinishedTalking && Time.time - lastTalkedTime > talkCooldownDuration)
        {
            isInteractable = true;
        }
    }

    private void FinishedTalking()
    {
        if (currentStage < BrianStage.TalkSecondTime)
        {
            currentStage++;
        }

        DialogManager.Instance.OnFinishedTalking -= FinishedTalking;
        DialogManager.Instance.OnNextMsg -= PlaySound;

        interactionIndicator.SetShowable(true);
        lastTalkedTime = Time.time;
        isFinishedTalking = true;
    }

    private List<SpeechDataSO> GetSpeechList() => msgList[(int)currentStage].SpeechList;

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


