using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerZone : MonoBehaviour
{
    public AudioSource areaAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSoundsManager.Instance.PlayAudio(areaAudio);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSoundsManager.Instance.StopAudio(areaAudio);
        }
    }
}