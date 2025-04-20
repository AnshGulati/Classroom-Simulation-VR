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

    //[Header("Outline Shader")]
    //[SerializeField] private float pulseSpeed = 1.5f;
    //[SerializeField] private float minOutlineWidth = 0.00001f;
    //[SerializeField] private float maxOutlineWidth = 0.0001f;

    //private List<Material> outlineMaterials = new List<Material>();
    //private bool isPulsing = false;

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
