using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSoundsManager : MonoBehaviour
{
    public static AudioSoundsManager Instance;

    private AudioSource currentAudio;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayAudio(AudioSource newAudio)
    {
        if (currentAudio != null && currentAudio != newAudio)
        {
            currentAudio.Stop();
        }

        if (newAudio != null && !newAudio.isPlaying)
        {
            currentAudio = newAudio;
            currentAudio.Play();
        }
    }

    public void StopAudio(AudioSource audio)
    {
        if (audio != null && audio.isPlaying)
        {
            audio.Stop();
            if (currentAudio == audio)
                currentAudio = null;
        }
    }
}
