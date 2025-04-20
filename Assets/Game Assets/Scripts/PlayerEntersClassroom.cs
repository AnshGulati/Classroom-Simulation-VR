using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntersClassroom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VoiceLinesManager voiceLinesManager;
    [SerializeField] private TextTypewriterAnim textTypewriterAnim;
    [SerializeField] private DoorLockAtMax doorLockAtMax;

    [Header("Chair Settings")]
    [SerializeField] private List<GameObject> chairs;
    [SerializeField] private List<GameObject> directionalArrows;
    [SerializeField] private float pulseSpeed = 1.5f;
    [SerializeField] private float minOutlineWidth = 0.00001f;
    [SerializeField] private float maxOutlineWidth = 0.0001f;

    [Header("Student Settings")]
    [SerializeField] private List<Transform> studentHeadsToLook; // The parts of the students that should rotate (e.g. head)
    [SerializeField] private float lookDuration = 2f;
    [SerializeField] private float delayBeforeChairHighlight = 2f;

    private List<Material> outlineMaterials = new List<Material>();
    private bool isPulsing = false;
    private Transform player;
    private Dictionary<Transform, Quaternion> originalRotations = new Dictionary<Transform, Quaternion>();

    private void Start()
    {
        foreach (Transform student in studentHeadsToLook)
        {
            originalRotations[student] = student.rotation;
        }

        foreach (GameObject arrow in directionalArrows)
        {
            arrow.SetActive(false);
        }

        foreach (GameObject chair in chairs)
        {
            foreach (Transform child in chair.transform)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    foreach (Material mat in renderer.materials)
                    {
                        if (mat.HasProperty("_OutlineWidth"))
                        {
                            outlineMaterials.Add(mat);
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPulsing)
        {
            player = other.transform;
            doorLockAtMax.TriggerDoorReset();

            StartCoroutine(HandleStudentLook());
            GetComponent<Collider>().enabled = false;
        }
    }

    private IEnumerator HandleStudentLook()
    {
        voiceLinesManager.TriggerLine3();
        textTypewriterAnim.NextLine();

        float rotateDuration = 1f;
        float elapsed = 0f;

        // Students look at player
        foreach (Transform student in studentHeadsToLook)
        {
            if (!originalRotations.ContainsKey(student))
                originalRotations[student] = student.rotation;
        }

        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotateDuration);

            foreach (Transform student in studentHeadsToLook)
            {
                Vector3 dir = (player.position - student.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
                student.rotation = Quaternion.Slerp(student.rotation, targetRotation, t);
            }

            yield return null;
        }

        yield return new WaitForSeconds(lookDuration);

        elapsed = 0f;

        // Reset rotations
        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotateDuration);

            foreach (Transform student in studentHeadsToLook)
            {
                if (originalRotations.TryGetValue(student, out Quaternion originalRot))
                {
                    student.rotation = Quaternion.Slerp(student.rotation, originalRot, t);
                }
            }

            yield return null;
        }

        // Delay then start chair highlight
        yield return new WaitForSeconds(delayBeforeChairHighlight);
        StartCoroutine(PulseOutline());

        foreach (GameObject arrow in directionalArrows)
        {
            arrow.SetActive(true);
        }
    }

    private IEnumerator PulseOutline()
    {
        voiceLinesManager.StartLine4Loop();
        textTypewriterAnim.NextLine();
        isPulsing = true;

        while (true)
        {
            float t = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            float outlineWidth = Mathf.Lerp(minOutlineWidth, maxOutlineWidth, t);

            foreach (Material mat in outlineMaterials)
            {
                mat.SetFloat("_OutlineWidth", outlineWidth);
            }

            yield return null;
        }
    }

    public void StopOutlineAndArrows()
    {
        StopCoroutine(PulseOutline());
        foreach (GameObject arrow in directionalArrows)
        {
            arrow.SetActive(false);
        }
    }
}