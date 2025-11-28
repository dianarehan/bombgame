using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("UI References")]
    public CanvasGroup fadeCanvasGroup;
    public TextMeshProUGUI loadingText;

    [Header("Settings")]
    [Tooltip("How fast the screen turns black (Make this small, e.g., 0.3)")]
    public float fadeInDuration = 0.3f; 

    [Tooltip("How slow the screen reveals the new level (Make this larger, e.g., 1.0)")]
    public float fadeOutDuration = 1.0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if(fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0;
            fadeCanvasGroup.blocksRaycasts = false;
        }
        if(loadingText != null) loadingText.gameObject.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        if(loadingText != null) loadingText.gameObject.SetActive(true);

        fadeCanvasGroup.blocksRaycasts = true;
        float timer = 0;
        
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeInDuration);
            yield return null;
        }
        
        fadeCanvasGroup.alpha = 1;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        while (!operation.isDone)
        {
            yield return null;
        }

        if(loadingText != null) loadingText.gameObject.SetActive(false);
        
        timer = 0;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeOutDuration);
            yield return null;
        }
        
        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.blocksRaycasts = false;
    }
}