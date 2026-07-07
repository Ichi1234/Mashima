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

    private float talkCooldownDuration = 1f;
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

        DialogManager.Instance.OpenDialogBox(npcName, GetSpeechList());
    }

    private void OnEnable()
    {
        DialogManager.Instance.OnFinishedTalking += FinishedTalking;
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

    private void OnDisable()
    {
        DialogManager.Instance.OnFinishedTalking -= FinishedTalking;
    }

    private void FinishedTalking()
    {
        if (currentStage < BrianStage.TalkSecondTime)
        {
            currentStage++;
        }
        lastTalkedTime = Time.time;
        isFinishedTalking = true;
    }

    private List<SpeechDataSO> GetSpeechList() => msgList[(int)currentStage].SpeechList;

}


