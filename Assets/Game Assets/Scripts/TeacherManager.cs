using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherManager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject teacher;
    public Animator teacherAnimator;
    public Transform moveTarget;
    
    public GameObject door;
    public Animator doorAnimator;
    
    public Transform xrRigTransform;

    [Header("UI")]
    public GameObject dialogUI;
    public GameObject textOneUI;
    public GameObject textTwoUI;

    [Header("Sound")]
    public AudioSource teacherFootsteps;
    [SerializeField] private VoiceLinesManager voiceLinesManager;

    [Header("Variables")]
    public float moveSpeed = 2f;
    public float startDelay = 1f;
    public float talkDelay = 3f;

    private bool shouldMove = false;
    private bool hasReached = false;

    private void Start()
    {
        teacher.SetActive(false);
        dialogUI.SetActive(false);
        textOneUI.SetActive(false);
        textTwoUI.SetActive(false);
    }

    public void TriggerTeacherEntrance()
    {
        StartCoroutine(EntranceSequence());
    }

    private IEnumerator EntranceSequence()
    {
        yield return new WaitForSeconds(startDelay);

        teacher.SetActive(true);

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }

        yield return new WaitForSeconds(1f);

        teacherAnimator.SetBool("isWalking", true);
        shouldMove = true;
        teacherFootsteps.Play();

        StartCoroutine(CloseDoorAfterDelay(2f));
    }

    private IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Close");
        }
    }

    private void Update()
    {
        if (shouldMove && !hasReached)
        {
            Vector3 direction = (moveTarget.position - teacher.transform.position).normalized;
            teacher.transform.position += direction * moveSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, targetRotation, Time.deltaTime * 5f);

            if (Vector3.Distance(teacher.transform.position, moveTarget.position) < 0.2f)
            {
                shouldMove = false;
                hasReached = true;
                teacherFootsteps.Stop();
                teacherAnimator.SetBool("isWalking", false);
                StartCoroutine(HandleArrival());
            }
        }
    }

    private IEnumerator HandleArrival()
    {
        yield return new WaitForSeconds(0.5f);
        Quaternion initialRotation = Quaternion.Euler(0, 0, 0);
        yield return StartCoroutine(SmoothRotate(initialRotation, 0.5f));

        yield return new WaitForSeconds(talkDelay);

        if (xrRigTransform != null)
        {
            Vector3 directionToXRRig = (xrRigTransform.position - teacher.transform.position).normalized;
            directionToXRRig.y = 0f;
            Quaternion lookAtRotation = Quaternion.LookRotation(directionToXRRig);

            yield return StartCoroutine(SmoothRotate(lookAtRotation, 0.5f));
        }

        teacherAnimator.SetBool("isTalking", true);
        voiceLinesManager.TriggerLine8();

        if (dialogUI != null)
        {
            dialogUI.SetActive(true);
        }
    }

    private IEnumerator SmoothRotate(Quaternion targetRotation, float duration)
    {
        Quaternion initialRotation = teacher.transform.rotation;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            teacher.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        teacher.transform.rotation = targetRotation;
    }

    public void StopTalking()
    {
        teacherAnimator.SetBool("isTalking", false);
        if (dialogUI != null)
            dialogUI.SetActive(false);
    }
}