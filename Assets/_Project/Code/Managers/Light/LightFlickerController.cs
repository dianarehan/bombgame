using System.Collections;
using System.Collections.Generic;
using System.Linq; // Used for easy random selection
using UnityEngine;

public class LightFlickerController : MonoBehaviour
{
    [Header("Lights Setup")]
    [Tooltip("The list of lights you want to control.")]
    public List<Light> lightsToFlicker;

    [Header("Flicker Timing")]
    [Tooltip("Minimum time to wait before starting a new flicker event.")]
    public float minWaitTime = 5f;
    [Tooltip("Maximum time to wait before starting a new flicker event.")]
    public float maxWaitTime = 10f;

    [Tooltip("Minimum duration for a single flicker event.")]
    public float minFlickerDuration = 1f;
    [Tooltip("Maximum duration for a single flicker event.")]
    public float maxFlickerDuration = 3f;

    [Header("Flicker Parameters")]
    [Tooltip("Minimum number of lights to flicker at one time.")]
    public int minLights = 1;
    [Tooltip("Maximum number of lights to flicker at one time.")]
    public int maxLights = 3;

    [Tooltip("The lowest intensity the light will flicker to.")]
    public float minFlickerIntensity = 0f;
    [Tooltip("The highest intensity the light will flicker to.")]
    public float maxFlickerIntensity = 2f;

    // Dictionary to store the original intensity of each light
    private Dictionary<Light, float> originalIntensities;
    
    // Set to keep track of lights currently flickering
    private HashSet<Light> lightsCurrentlyFlickering;

    void Start()
    {
        // Initialize the dictionary and set
        originalIntensities = new Dictionary<Light, float>();
        lightsCurrentlyFlickering = new HashSet<Light>();

        // Store the original intensity for each light
        foreach (Light light in lightsToFlicker)
        {
            if (light != null)
            {
                originalIntensities[light] = light.intensity;
            }
        }

        // Start the main loop that triggers flicker events
        StartCoroutine(FlickerLoop());
    }

    /// <summary>
    /// Main loop that waits for a random time and then starts a flicker event.
    /// </summary>
    private IEnumerator FlickerLoop()
    {
        // This loop runs forever
        while (true)
        {
            // Wait for a random amount of time
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Start a new flicker event
            StartCoroutine(StartFlickerEvent());
        }
    }

    /// <summary>
    /// Selects random lights and starts the flicker coroutine for each one.
    /// </summary>
    private IEnumerator StartFlickerEvent()
    {
        // Determine how many lights to flicker, clamped by available lights
        int lightCount = Random.Range(minLights, maxLights + 1);
        lightCount = Mathf.Clamp(lightCount, 0, lightsToFlicker.Count);

        // Get a list of available lights (those not already flickering)
        var availableLights = lightsToFlicker
            .Where(light => light != null && !lightsCurrentlyFlickering.Contains(light))
            .ToList();
            
        // If no lights are available, just exit
        if (availableLights.Count == 0)
        {
            yield break;
        }

        // Randomly select lights from the available list
        // We use OrderBy with a random value as a simple way to shuffle
        var selectedLights = availableLights
            .OrderBy(x => Random.value)
            .Take(lightCount)
            .ToList();

        // Get a random duration for this specific event
        float flickerDuration = Random.Range(minFlickerDuration, maxFlickerDuration);

        // Start the flicker coroutine for each selected light
        foreach (Light light in selectedLights)
        {
            StartCoroutine(FlickerLight(light, flickerDuration));
        }
    }

    /// <summary>
    /// Handles the flickering of a single light for a set duration.
    /// </summary>
    private IEnumerator FlickerLight(Light light, float duration)
    {
        // Mark this light as "flickering"
        lightsCurrentlyFlickering.Add(light);

        // Get the original intensity from our dictionary
        float originalIntensity = originalIntensities[light];

        float timer = 0f;
        while (timer < duration)
        {
            // Set a random intensity
            light.intensity = Random.Range(minFlickerIntensity, maxFlickerIntensity);

            // Wait for a very short, random time to create the "flicker"
            float flickerSpeed = Random.Range(0.05f, 0.15f);
            yield return new WaitForSeconds(flickerSpeed);

            // Update the timer
            timer += flickerSpeed;
        }

        // Flicker duration is over, restore the original intensity
        light.intensity = originalIntensity;

        // Mark this light as "no longer flickering"
        lightsCurrentlyFlickering.Remove(light);
    }
}