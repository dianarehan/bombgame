using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

// This makes our custom subtitle data visible in the Inspector
[System.Serializable]
public class SubtitleLine
{
    [TextArea]
    public string text;
    public float duration;
}

public class SubtitlePlayer : MonoBehaviour
{
    // Assign your UI Text component here in the Inspector
    [SerializeField] private TextMeshProUGUI subtitleTextUI;

    // This is where you'll write your subtitles in the Inspector
    [SerializeField] private List<SubtitleLine> lines;

    private Coroutine _subtitleCoroutine;

    
    public void StartSubtitles()
    {
        // Start the coroutine to play subtitles
        _subtitleCoroutine = StartCoroutine(PlaySubtitles());
    }

    private void OnDisable()
    {
        // IMPORTANT: Always stop the coroutine when the object is disabled.
        // This prevents errors if it gets turned off mid-sequence.
        if (_subtitleCoroutine != null)
        {
            StopCoroutine(_subtitleCoroutine);
        }
    }

    private IEnumerator PlaySubtitles()
    {
        // Make sure the text is active and initially empty
        subtitleTextUI.gameObject.SetActive(true);
        subtitleTextUI.text = "";

        // Loop through each line in our list
        foreach (var line in lines)
        {
            // Set the text
            subtitleTextUI.text = line.text;

            // Wait for the specified duration before moving to the next line
            yield return new WaitForSeconds(line.duration);
        }

        // After the loop finishes, clear the text and hide the UI object
        subtitleTextUI.text = "";
        subtitleTextUI.gameObject.SetActive(false);
    }
}