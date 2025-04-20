using Meta.WitAi.CallbackHandlers;
using Oculus.Voice;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class VoiceManager : MonoBehaviour
{
    [System.Serializable]
    public class ChairData
    {
        public int chairID;
        public TextMeshProUGUI transcriptionText;
    }

    [Header("Wit Configuration")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;
    [SerializeField] private WitResponseMatcher witResponseMatcher;

    [Header("UI - Transcript")]
    [SerializeField] private List<ChairData> chairList;
    private Dictionary<int, TextMeshProUGUI> chairTextMap;
    private int currentChairID = -1;

    [Header("Voice Events")]
    [SerializeField] private UnityEvent wakeWordDetected;
    [SerializeField] private UnityEvent<string> completeTranscription;

    private bool _voiceCommandReady;

    private void Awake()
    {
        chairTextMap = new Dictionary<int, TextMeshProUGUI>();
        foreach (var chair in chairList)
        {
            chairTextMap[chair.chairID] = chair.transcriptionText;
        }

        appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(ReactivateVoice);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnFullTranscription);

        var eventField = typeof(WitResponseMatcher).GetField("onMultiValueEvent", BindingFlags.NonPublic | BindingFlags.Instance);
        if (eventField != null && eventField.GetValue(witResponseMatcher) is MultiValueEvent onMultiValueEvent)
        {
            onMultiValueEvent.AddListener(WakeWordDetected);
        }

        appVoiceExperience.Activate();
    }

    public void SetCurrentChairID(int chairID)
    {
        currentChairID = chairID;
    }

    private void OnDestroy()
    {
        appVoiceExperience.VoiceEvents.OnRequestCompleted.RemoveListener(ReactivateVoice);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnFullTranscription);

        var eventField = typeof(WitResponseMatcher).GetField("onMultiValueEvent", BindingFlags.NonPublic | BindingFlags.Instance);
        if (eventField != null && eventField.GetValue(witResponseMatcher) is MultiValueEvent onMultiValueEvent)
        {
            onMultiValueEvent.RemoveListener(WakeWordDetected);
        }
    }

    private void ReactivateVoice() => appVoiceExperience.Activate();

    private void WakeWordDetected(string[] arg0)
    {
        _voiceCommandReady = true;
        wakeWordDetected?.Invoke();
    }

    private void OnPartialTranscription(string transcription)
    {
        if (!_voiceCommandReady || currentChairID == -1) return;
        if (chairTextMap.TryGetValue(currentChairID, out var textUI))
        {
            textUI.text = transcription;
        }
    }

    private void OnFullTranscription(string transcription)
    {
        if (!_voiceCommandReady || currentChairID == -1) return;
        _voiceCommandReady = false;
        completeTranscription?.Invoke(transcription);

        if (chairTextMap.TryGetValue(currentChairID, out var textUI))
        {
            textUI.text = transcription;
        }
    }
}
