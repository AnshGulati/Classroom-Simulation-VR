using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLinesManager : MonoBehaviour
{
    [System.Serializable]
    public class VoiceLine
    {
        public AudioClip clip;
        public bool playOnlyOnce = true;
        [HideInInspector] public bool hasPlayed = false;
    }

    public AudioSource audioSource;

    [Header("Voice Lines")]
    public VoiceLine line1;
    public VoiceLine line2;
    public VoiceLine line3;
    public VoiceLine line4;
    public VoiceLine line5;
    public VoiceLine line6;
    public VoiceLine line6_2;
    public VoiceLine line7;
    public VoiceLine line8;
    public VoiceLine line9;
    public VoiceLine line10;
    public VoiceLine line11;
    public VoiceLine line12;

    private Coroutine line4LoopCoroutine;

    //[SerializeField] private GameObject speakInputExperience;
    public ChairSitTrigger chairSitTrigger;

    public static VoiceLinesManager Instance;
    private ChairSitTrigger currentChairSitTrigger;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(PlayVoiceLineWithDelay(line1, 3f)); // Line 1

        //speakInputExperience.SetActive(false);
        for (int i = 0; i < GameTransitionManager.Instance.speakInputExperienceUI.Length; i++)
        {
            GameTransitionManager.Instance.ShowExperienceUI(i, false);
        }
    }

    public void SetCurrentChair(ChairSitTrigger chair)
    {
        currentChairSitTrigger = chair;
    }

    private IEnumerator PlayVoiceLineWithDelay(VoiceLine voiceLine, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayVoiceLine(voiceLine);
    }

    public void PlayVoiceLine(VoiceLine voiceLine)
    {
        if (voiceLine == null || voiceLine.clip == null)
            return;

        if (voiceLine.playOnlyOnce && voiceLine.hasPlayed)
            return;

        audioSource.clip = voiceLine.clip;
        audioSource.Play();
        voiceLine.hasPlayed = true;
    }

    public void StopVoiceLine()
    {
        audioSource.Stop();
    }

    public void TriggerLine3()
    {
        PlayVoiceLine(line3); // One-time trigger line
    }

    public void StartLine4Loop()
    {
        if (line4LoopCoroutine == null)
            line4LoopCoroutine = StartCoroutine(Line4Repeater());
    }

    private IEnumerator Line4Repeater()
    {
        //yield return new WaitForSeconds(5f); // Delay after line 3
        while (!line5.hasPlayed)
        {
            PlayVoiceLine(line4);
            yield return new WaitForSeconds(line4.clip.length + 5f); // Wait and repeat
        }
    }

    public void TriggerLine5()
    {
        if (line4LoopCoroutine != null)
        {
            StopCoroutine(line4LoopCoroutine);
            line4LoopCoroutine = null;
        }

        PlayVoiceLine(line5);

        StartCoroutine(HandleLine6Sequence());
    }

    private IEnumerator HandleLine6Sequence()
    {
        float delayToLine6 = line5.clip.length + 12f;
        yield return new WaitForSeconds(delayToLine6);

        PlayVoiceLine(line6);
        yield return new WaitForSeconds(line6.clip.length + 5f);

        PlayVoiceLine(line6_2);
        yield return new WaitForSeconds(line6_2.clip.length);

        //// Enable the object after line6_2 ends
        //if (speakInputExperience != null)
        //    speakInputExperience.SetActive(true);

        if (currentChairSitTrigger != null)
            GameTransitionManager.Instance.ShowExperienceUI(currentChairSitTrigger.chairID, true);

        GameTransitionManager.Instance.TriggerStudentTalking(currentChairSitTrigger.chairID);
    }

    // Functions to be called externally:
    public void TriggerLine7() => PlayVoiceLine(line7);
    public void TriggerLine8() => PlayVoiceLine(line8);
    public void TriggerLine9() => StartCoroutine(PlayVoiceLineWithDelay(line9, 3f));
    public void TriggerLine10() => StartCoroutine(PlayVoiceLineWithDelay(line10, 3f));
    public void TriggerLine11() => StartCoroutine(PlayVoiceLineWithDelay(line11, 3f));
    public void TriggerLine12() => PlayVoiceLine(line12);
}
