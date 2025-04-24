using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerFootstepsSound : MonoBehaviour
{
    public DynamicMoveProvider moveProvider;
    public AudioSource footstepAudio;
    public float movementThreshold = 0.01f;
    public SitManager sitManager;
    public PlayerFinalSpeechHandler playerFinalSpeechHandler;

    private void Update()
    {
        if (sitManager != null && sitManager.isSitting)
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
            return;
        }

        if (playerFinalSpeechHandler != null && playerFinalSpeechHandler.isFinalSequenceStarted)
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
            return;
        } 

        Vector2 moveInput = moveProvider.leftHandMoveAction.action.ReadValue<Vector2>();

        bool isMoving = moveInput.magnitude > movementThreshold;

        if (isMoving)
        {
            if (!footstepAudio.isPlaying)
                footstepAudio.Play();
        }
        else
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
        }
    }
}
