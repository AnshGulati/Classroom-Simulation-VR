using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerFinalSpeechHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SitManager sitManager;
    [SerializeField] private GameObject standUpUI;
    [SerializeField] private Transform speechMarker;
    [SerializeField] private GameObject markerArrow;
    [SerializeField] private GameObject speechUI;
    [SerializeField] private GameObject speechText;
    [SerializeField] private VoiceLinesManager voiceLinesManager;
    [SerializeField] private Transform lookAtTarget;
    [SerializeField] private float rotationSpeed = 3f;
    public Image holdCircleUI;

    [Header("Students List")]
    [SerializeField] private List<GameObject> studentList;

    [Header("Settings")]
    [SerializeField] private float markerReachDistance = 0.1f;
    [SerializeField] private float finalSpeechDelay = 2f;

    private bool hasStoodUp = false;
    private bool reachedMarker = false;
    private bool isFinalSequenceStarted = false;

    private float holdTimer = 0f;
    private float requiredHoldTime = 1f;

    private ChairSitTrigger currentChair;

    private Transform playerRig;

    public static PlayerFinalSpeechHandler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        playerRig = GameTransitionManager.Instance.xrRig.transform;
        standUpUI.SetActive(true);
        speechMarker.gameObject.SetActive(false);
        speechUI.SetActive(false);
        markerArrow.SetActive(false);
    }

    private void Update()
    {
        if (!hasStoodUp && sitManager != null && sitManager.CanSit())
        {
            holdTimer += Time.deltaTime;
            holdCircleUI.fillAmount = holdTimer / requiredHoldTime;

            if (holdTimer >= requiredHoldTime)
            {
                HandleStandUp();
                holdTimer = 0f;
                holdCircleUI.fillAmount = 0f;
            }
        }

        if (hasStoodUp && !reachedMarker)
        {
            float distance = Vector3.Distance(playerRig.position, speechMarker.position);
            if (distance <= markerReachDistance)
            {
                HandleReachedMarker();
            }
        }

        if (reachedMarker && !isFinalSequenceStarted)
        {
            RotatePlayerTowards(lookAtTarget.position);
        }
    }

    private void HandleStandUp()
    {
        sitManager.Stand();
        standUpUI.SetActive(false);
        speechMarker.gameObject.SetActive(true);
        markerArrow.SetActive(true);
        hasStoodUp = true;
    }

    public void SetCurrentChair(ChairSitTrigger chair)
    {
        currentChair = chair;
        standUpUI = chair.standUpUI.gameObject;
        holdCircleUI = chair.holdCircleUI;
        sitManager = chair.sitManager;
    }

    private void HandleReachedMarker()
    {
        reachedMarker = true;
        speechMarker.gameObject.SetActive(false);
        markerArrow.SetActive(false);
        speechUI.SetActive(true);
        StartCoroutine(StartFinalSpeech());
    }

    private IEnumerator StartFinalSpeech()
    {
        isFinalSequenceStarted = true;
        GameTransitionManager.Instance.ToggleMovement(false);

        // Text Input/Read Logic
        speechText.GetComponent<TextTypewriterAnim>().NextLine();

        yield return new WaitForSeconds(20f);

        speechUI.SetActive(false);
        TriggerStudentClaps();
        voiceLinesManager.TriggerLine12();

        yield return new WaitForSeconds(voiceLinesManager.line12.clip.length+4f);
        EndGame();
    }

    private void RotatePlayerTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - playerRig.position).normalized;
        direction.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(direction);
        playerRig.rotation = Quaternion.Slerp(playerRig.rotation, targetRot, Time.deltaTime * rotationSpeed);
    }

    private void TriggerStudentClaps()
    {
        foreach (var student in studentList)
        {
            Animator animator = student.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("Clapping", true);
            }
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Finished.");
        // Move to next scene
        SceneManager.LoadScene("1 Start Scene");
    }
}
