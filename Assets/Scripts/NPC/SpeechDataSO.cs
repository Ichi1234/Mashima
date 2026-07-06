using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName ="NPC Speech")]
public class SpeechDataSO : ScriptableObject
{
    public string speechText;
    public AudioClip speechSound;
}
