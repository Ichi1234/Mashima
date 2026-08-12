using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName ="NPC Speech")]
public class SpeechDataSO : ScriptableObject
{
    public string speechEnglishText;
    public string speechJapaneseText;
    public AudioClip speechSound;
}
