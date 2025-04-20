using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableHandGrabbingModel : MonoBehaviour
{
    public GameObject leftHandModel;
    public GameObject rightHandModel;

    private void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(HideGrabbingHand);
        grabInteractable.selectExited.AddListener(ShowGrabbingHand);
    }

    public void HideGrabbingHand(SelectEnterEventArgs args)
    {
        GameObject interactorObject = args.interactorObject.transform.gameObject; // This is the hand/controller

        if (interactorObject.CompareTag("Left Hand"))
        {
            leftHandModel.SetActive(false);
        }
        else if (interactorObject.CompareTag("Right Hand"))
        {
            rightHandModel.SetActive(false);
        }
    }

    public void ShowGrabbingHand(SelectExitEventArgs args)
    {
        GameObject interactorObject = args.interactorObject.transform.gameObject;

        if (interactorObject.CompareTag("Left Hand"))
        {
            leftHandModel.SetActive(true);
        }
        else if (interactorObject.CompareTag("Right Hand"))
        {
            rightHandModel.SetActive(true);
        }
    }
}
