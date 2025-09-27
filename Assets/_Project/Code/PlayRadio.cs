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

    [Header("Object Activation")]
    [Tooltip("The object to activate when the radio sound ends.")]
    [SerializeField] private GameObject objectToActivate;

    // Private variables
    private AudioSource audioSource;
    private Vector3 originalPosition;
    private bool isRadioOn = false;
    private Coroutine musicCoroutine;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.position;
    }

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
            audioSource.loop = false;
            audioSource.Play();

            // Wait for the audio to finish playing
            yield return new WaitForSeconds(radioMusicLoop.length);

            // Activate the object when the sound ends (if it's assigned and radio is still on)
            if (objectToActivate != null && isRadioOn)
            {
                objectToActivate.SetActive(true);
                Debug.Log("Object activated after radio sound ended.");
            }
        }
    }
}