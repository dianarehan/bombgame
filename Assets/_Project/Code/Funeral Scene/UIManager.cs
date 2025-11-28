using UnityEngine;
using TMPro;
using System.Collections;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [Tooltip("Drag your TextMeshPro object here.")]
    public TextMeshProUGUI messageText;
    
    [Tooltip("How long the message stays on screen.")]
    public float messageDuration = 1f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if(messageText != null) messageText.text = "";
    }

    public void ShowMessage(string text)
    {
        if (messageText == null) return;
        
        StopAllCoroutines();
        StartCoroutine(DisplayMessageRoutine(text));
    }

    private IEnumerator DisplayMessageRoutine(string text)
    {
        messageText.text = text;
        messageText.alpha = 1f;

        yield return new WaitForSeconds(messageDuration);

        float timer = 0;
        while(timer < 1f)
        {
            timer += Time.deltaTime;
            messageText.alpha = Mathf.Lerp(1f, 0f, timer);
            yield return null;
        }
        
        messageText.text = "";
    }
}