using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VoiceLinesManager voiceLinesManager;
    [SerializeField] private TextTypewriterAnim textTypewriterAnim;
    [SerializeField] private PlayerEntersClassroom playerEntersClassroom;
    [SerializeField] private Transform standingStudentsParent;
    [SerializeField] private Transform sittingStudentsParent;

    [Header("Sitting Settings")]
    [SerializeField] private float sittingHeight = -0.3f;
    [SerializeField] private float transitionSpeed = 5f;
    [SerializeField] private Transform lookAtTarget;

    private bool isSitting = false;
    private Coroutine sittingCoroutine;

    private void Start()
    {
        ActivateChildren(standingStudentsParent);
        DisableChildren(sittingStudentsParent);
    }

    public void Sit(Transform sitPoint)
    {
        if (isSitting) return;

        isSitting = true;
        voiceLinesManager.TriggerLine5();
        textTypewriterAnim.NextLine();
        ActivateChildren(sittingStudentsParent);
        DisableChildren(standingStudentsParent);
        playerEntersClassroom.StopOutlineAndArrows();

        GameTransitionManager.Instance.ToggleMovement(false);

        if (sittingCoroutine != null)
            StopCoroutine(sittingCoroutine);

        sittingCoroutine = StartCoroutine(TransitionToSitting(sitPoint));
    }

    public void Stand()
    {
        if (!isSitting) return;

        isSitting = false;
        GameTransitionManager.Instance.ToggleMovement(true);

        Transform rig = GameTransitionManager.Instance.xrRig.transform;
        Vector3 standPosition = rig.position - new Vector3(0, sittingHeight, 0); // Move player up

        rig.position = standPosition;
    }

    public bool CanSit()
    {
        var left = GameTransitionManager.Instance.leftController;
        var right = GameTransitionManager.Instance.rightController;

        if (left == null || right == null)
            return false;

        return left.selectAction.action.ReadValue<float>() > 0.8f &&
               right.selectAction.action.ReadValue<float>() > 0.8f;
    }

    private IEnumerator TransitionToSitting(Transform sitPoint)
    {
        var rig = GameTransitionManager.Instance.xrRig.transform;

        Vector3 targetPosition = sitPoint.position + new Vector3(0, sittingHeight, 0);

        Vector3 directionToTarget = (lookAtTarget.position - rig.position).normalized;
        directionToTarget.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        while (Vector3.Distance(rig.position, targetPosition) > 0.01f || Quaternion.Angle(rig.rotation, targetRotation) > 1f)
        {
            rig.position = Vector3.Lerp(rig.position, targetPosition, Time.deltaTime * transitionSpeed);
            rig.rotation = Quaternion.Slerp(rig.rotation, targetRotation, Time.deltaTime * transitionSpeed);
            yield return null;
        }

        rig.position = targetPosition;
        rig.rotation = targetRotation;
    }

    private void ActivateChildren(Transform parentObject)
    {
        foreach (Transform child in parentObject)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void DisableChildren(Transform parentObject)
    {
        foreach (Transform child in parentObject)
        {
            child.gameObject.SetActive(false);
        }
    }
}
