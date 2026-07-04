using System;
using UnityEngine;

[Serializable]
public class AudioClipData
{
    public string audioName;
    public AudioClip clip;
    [Range(0f, 1f)] public float maxVolume = 1f;
    [Header("3D sound setting")]
    [Range(0f, 360f)] public float spread = 0;
    public float minDistance = 1;
    public float maxDistance = 20;
    public float pitch = 1;
}
