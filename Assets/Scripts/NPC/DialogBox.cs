using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
    [SerializeField] private TextMeshPro npcName;
    [SerializeField] private TextMeshPro message;

    [SerializeField] private List<string> msgLists;
    [SerializeField] private List<float> nextMsgTimeStamps;

    public void SetNpcName(string name) => npcName.text = name;

    public void SetMsg(string newMsg) => message.text = newMsg;
}
