using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChairSitTrigger : MonoBehaviour
{
    public int chairID;
    public Transform sitPoint;
    public SitManager sitManager;
    public Image holdCircleUI;

    private bool playerInTrigger = false;
    private float holdTimer = 0f;
    private float requiredHoldTime = 1f;

    public GameObject standUpUI;

    [SerializeField] private VoiceManager voiceManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            voiceManager.SetCurrentChairID(chairID);
            VoiceLinesManager.Instance.SetCurrentChair(this);
            playerInTrigger = true;
            GameTransitionManager.Instance.ShowInstructionUI(chairID, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            holdTimer = 0f;
            holdCircleUI.fillAmount = 0f;
            GameTransitionManager.Instance.ShowInstructionUI(chairID, false);
        }
    }

    private void Update()
    {
        if (playerInTrigger && sitManager != null && sitManager.CanSit())
        {
            holdTimer += Time.deltaTime;
            holdCircleUI.fillAmount = holdTimer / requiredHoldTime;

            if (holdTimer >= requiredHoldTime)
            {
                sitManager.Sit(sitPoint);
                PlayerFinalSpeechHandler.Instance.SetCurrentChair(this);
                GameTransitionManager.Instance.ShowInstructionUI(chairID, false);
                holdTimer = 0f;
                holdCircleUI.fillAmount = 0f;
            }
        }
        else
        {
            holdTimer = 0f;
            holdCircleUI.fillAmount = 0f;
        }
    }
}