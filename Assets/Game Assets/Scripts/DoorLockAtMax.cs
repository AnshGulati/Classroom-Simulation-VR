using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorLockAtMax : MonoBehaviour
{
    [Header("Assign Components")]
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject openUI;
    [SerializeField] private List<GameObject> originalDoors;
    [SerializeField] private List<GameObject> resetDoors;

    [Header("Settings")]
    [SerializeField] private float lockThreshold = 2f; // degrees away from max limit to lock
    [SerializeField] private float resetDelay = 2f;

    public bool isLocked = false;
    private float maxLimit;
    private float minLimit;

    private void Start()
    {
        maxLimit = hinge.limits.max;
        minLimit = hinge.limits.min;

        foreach (GameObject door in originalDoors)
        {
            door.SetActive(true);
        }

        foreach (GameObject door in resetDoors)
        {
            door.SetActive(false);
        }
    }

    private void Update()
    {
        if (isLocked) return;

        float currentAngle = hinge.angle;

        if (Mathf.Abs(currentAngle - maxLimit) <= lockThreshold &&
            Mathf.Abs(currentAngle - minLimit) > lockThreshold + 5f)
        {
            LockDoor();
        }
    }

    private void LockDoor()
    {
        isLocked = true;
        rb.isKinematic = true;
        grabInteractable.enabled = false;
        openUI.SetActive(false);

        Debug.Log($"{gameObject.name} locked at max hinge angle.");
    }

    public void TriggerDoorReset()
    {
        if (isLocked)
        {
            StartCoroutine(ResetDoorCoroutine());
        }
    }

    private IEnumerator ResetDoorCoroutine()
    {
        yield return new WaitForSeconds(resetDelay);

        foreach (GameObject door in originalDoors)
        {
            door.SetActive(false);
        }

        foreach (GameObject door in resetDoors)
        {
            door.SetActive(true);
        }

        Debug.Log($"{gameObject.name} has been reset to its original rotation.");
    }
}