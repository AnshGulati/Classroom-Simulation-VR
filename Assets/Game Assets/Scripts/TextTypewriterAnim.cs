using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTypewriterAnim : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    public string[] stringArray;

    [SerializeField] private float timeBtwnChars = 0.05f;

    private int i = 0;
    private bool isTyping = false;
    private bool readyForNext = false;

    private void Start()
    {
        StartTyping();
    }

    private void StartTyping()
    {
        if (i < stringArray.Length)
        {
            _textMeshPro.text = stringArray[i];
            StartCoroutine(TextVisible());
        }
    }

    private IEnumerator TextVisible()
    {
        isTyping = true;
        readyForNext = false;

        _textMeshPro.ForceMeshUpdate();
        int totalVisibleChars = _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (counter <= totalVisibleChars)
        {
            _textMeshPro.maxVisibleCharacters = counter;
            counter++;
            yield return new WaitForSeconds(timeBtwnChars);
        }

        isTyping = false;
        readyForNext = true;
    }

    public void NextLine()
    {
        if (!isTyping && readyForNext && i < stringArray.Length - 1)
        {
            i++;
            StartTyping();
        }
    }
}
