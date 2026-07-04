using UnityEngine;

[DefaultExecutionOrder(-100)]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioDatabase audioDB;
    public static AudioManager Instance;


    private void Awake()
    {
        Instance = this;
    }

    public void PlaySFX(string soundName, AudioSource sfxSource, bool randomPitch = false)
    {
        AudioClipData data = audioDB.GetAudio(soundName);
        if (data == null) return;
        if (data.clip == null) return;

        sfxSource.pitch = randomPitch ? Random.Range(0.95f, 1.1f) : data.pitch;
        sfxSource.spread = data.spread;
        sfxSource.maxDistance = data.maxDistance;
        sfxSource.minDistance = data.minDistance;

        sfxSource.PlayOneShot(data.clip, data.maxVolume);
    }

}
