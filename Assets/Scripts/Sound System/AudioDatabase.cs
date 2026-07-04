using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio DB")]
public class AudioDatabase : ScriptableObject
{
    public List<AudioClipData> player;
    public List<AudioClipData> pursuer;
    public List<AudioClipData> env;

    private Dictionary<string, AudioClipData> clipCollection;

    private void OnEnable()
    {
        clipCollection = new Dictionary<string, AudioClipData>();

        AddToCollection(player);
        AddToCollection(pursuer);
        AddToCollection(env);
    }

    private void AddToCollection(List<AudioClipData> listToAdd)
    {
        foreach (var data in listToAdd)
        {
            if (data != null && clipCollection.ContainsKey(data.audioName) == false)
            {
                clipCollection.Add(data.audioName, data);
            }
        }
    }

    public AudioClipData GetAudio(string groupName)
    {
        return clipCollection.TryGetValue(groupName, out var data) ? data : null;

    }
}
