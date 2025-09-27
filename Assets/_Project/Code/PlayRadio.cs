using UnityEngine;
using System.Collections;

public class PlayRadio : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("The sound to play when the radio is first clicked on (e.g., a static crackle).")]
    [SerializeField] private AudioClip turnOnSound;

    [Tooltip("The main audio loop that plays continuously (e.g., music, news broadcast).")]
    [SerializeField] private AudioClip radioMusicLoop;

    [Header("Vibration Effect")]
    [Tooltip("How intense the shaking effect should be.")]
    [SerializeField] private float vibrationIntensity = 0.05f;

    [Tooltip("How long the radio should shake when turned on.")]
    [SerializeField] private float vibrationDuration = 0.5f;

    [Header("Visual Effect (Glow)")]
    [Tooltip("The Renderer of the part of the radio that should glow (e.g., a small light or dial).")]
    [SerializeField] private Renderer radioLightRenderer;

    [Tooltip("The color the light should glow when the radio is on.")]
    [SerializeField] private Color emissionColor = new Color(1.0f, 0.8f, 0.4f); // A warm yellow/orange

    // Private variables
    private AudioSource audioSource;
    private Vector3 originalPosition;
    private bool isRadioOn = false;
    private Coroutine musicCoroutine;

    // The name of the emission property in Unity's standard shaders
    private const string EMISSION_PROPERTY = "_EmissionColor";

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Store the radio's starting position to reset it after vibration
        originalPosition = transform.position;

        // Ensure the radio light material can have emission
        if (radioLightRenderer != null)
        {
            radioLightRenderer.material.EnableKeyword("_EMISSION");
            // Start with the light off
            radioLightRenderer.material.SetColor(EMISSION_PROPERTY, Color.black);
        }
    }

    // This function is called by Unity when the GameObject's collider is clicked
    void OnMouseDown()
    {
        // Toggle the radio's state
        if (isRadioOn)
        {
            TurnOffRadio();
        }
        else
        {
            TurnOnRadio();
        }
    }

    private void TurnOnRadio()
    {
        isRadioOn = true;
        Debug.Log("Radio turning ON.");

        // --- EFFECTS ---

        // 1. Play the initial "turn on" sound effect
        if (turnOnSound != null)
        {
            audioSource.PlayOneShot(turnOnSound);
        }

        // 2. Start the vibration effect
        StartCoroutine(VibrateRadio());

        // 3. Turn on the glowing light
        if (radioLightRenderer != null)
        {
            radioLightRenderer.material.SetColor(EMISSION_PROPERTY, emissionColor);
        }

        // 4. Start playing the main music loop after a short delay
        if (radioMusicLoop != null)
        {
            // Start a new coroutine to play music after the turn-on sound finishes
            float delay = turnOnSound ? turnOnSound.length : 0.1f;
            musicCoroutine = StartCoroutine(PlayMusicLoop(delay));
        }
    }

    private void TurnOffRadio()
    {
        isRadioOn = false;
        Debug.Log("Radio turning OFF.");

        // Stop all running effects on this script
        StopAllCoroutines();
        audioSource.Stop();

        // Reset the radio's position in case it was vibrating
        transform.position = originalPosition;

        // Turn off the glowing light
        if (radioLightRenderer != null)
        {
            radioLightRenderer.material.SetColor(EMISSION_PROPERTY, Color.black);
        }
    }

    // Coroutine for the vibration effect
    private IEnumerator VibrateRadio()
    {
        float elapsedTime = 0f;

        while (elapsedTime < vibrationDuration)
        {
            // Create a random offset from the original position
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f) * vibrationIntensity,
                Random.Range(-1f, 1f) * vibrationIntensity,
                0 // Assuming a 2D-style shake, you can add Z axis if needed
            );

            transform.position = originalPosition + randomOffset;

            elapsedTime += Time.deltaTime;

            // Wait until the next frame before continuing the loop
            yield return null;
        }

        // Reset the position to its original state after vibrating
        transform.position = originalPosition;
    }

    // Coroutine to play the main music after a delay
    private IEnumerator PlayMusicLoop(float delay)
    {
        yield return new WaitForSeconds(delay);

        // This check ensures that if the radio was turned off during the delay, the music doesn't start
        if (isRadioOn)
        {
            audioSource.clip = radioMusicLoop;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
