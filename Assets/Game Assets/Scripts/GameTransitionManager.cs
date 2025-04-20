using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameTransitionManager : MonoBehaviour
{
    public static GameTransitionManager Instance;

    [Header("XR Setup")]
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public DynamicMoveProvider moveProvider;
    public TeleportationProvider teleportationProvider;
    public GameObject xrRig;
    public XRRayInteractor rightRayInteractor;

    [Header("Instruction Panels")]
    public GameObject[] instructionPanels;
    public GameObject[] speakInputExperienceUI;

    [Header("Student Animation Controllers")]
    public TalkingAnimationController[] talkingControllers;
    public float delayBeforeTalking = 2f;

    public TeacherManager teacherManager;

    public void TriggerStudentTalking(int index)
    {
        if (index >= 0 && index < talkingControllers.Length)
        {
            talkingControllers[index].StartTalkingDelayed(delayBeforeTalking);
        }

        
    }


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        foreach (GameObject panel in instructionPanels)
            panel.SetActive(false);

        teacherManager.TriggerTeacherEntrance();
    }

    public void ShowInstructionUI(int chairID, bool show)
    {
        if (chairID >= 0 && chairID < instructionPanels.Length)
        {
            instructionPanels[chairID].gameObject.SetActive(show);
        }
    }

    public void ShowExperienceUI(int chairID, bool show)
    {
        if (chairID >= 0 && chairID < speakInputExperienceUI.Length)
        {
            speakInputExperienceUI[chairID].gameObject.SetActive(show);
        }
    }

    public void ToggleMovement(bool enable)
    {
        if (moveProvider) moveProvider.enabled = enable;
        if (teleportationProvider) teleportationProvider.enabled = enable;
        if (rightRayInteractor) rightRayInteractor.enabled = enable;
    }
}
