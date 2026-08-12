using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpeechGroup
{
    public List<SpeechDataSO> SpeechList;
    public bool loop = false;
}