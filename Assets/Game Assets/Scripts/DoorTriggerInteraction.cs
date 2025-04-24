using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject OpenInstructionUI;

    [Header("References")]
    [SerializeField] private VoiceLinesManager voiceManager;
    [SerializeField] private TextTypewriterAnim textTypewriterAnim;
    [SerializeField] private DoorLockAtMax doorLockAtMax;

    private void Start()
    {
        OpenInstructionUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !doorLockAtMax.isLocked)
        {
            OpenInstructionUI.SetActive(true);
            textTypewriterAnim.NextLine();
            voiceManager.PlayVoiceLine(voiceManager.line2); // Line 2
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !doorLockAtMax.isLocked)
        {
            OpenInstructionUI.SetActive(false);
            voiceManager.StopVoiceLine();
        }
    }
}
