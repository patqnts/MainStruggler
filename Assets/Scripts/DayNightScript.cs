using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class DayNighScript : MonoBehaviour
{
    [SerializeField] private float dayLength = 120f; // Length of a day in seconds
    [SerializeField] private float transitionLength = 10f; // Length of transition between day and night in seconds
    [SerializeField] private Volume globalVolume; // Global volume with override color adjustment
    private float timeOfDay = 0f; // Current time of day
    private float timeScale = 1f; // Time scale for the day and night cycle
    private bool isDay = true; // Whether it is currently day or night

    private void Start()
    {
        globalVolume.weight = 0f; // Set initial weight to day
    }

    private void Update()
    {
        // Update the time of day
        timeOfDay += Time.deltaTime * timeScale;
        if (timeOfDay >= dayLength)
        {
            timeOfDay = 0f;
        }

        // Check if it's time to transition between day and night
        if (timeOfDay >= dayLength - transitionLength && isDay)
        {
            isDay = false;
            StartCoroutine(TransitionVolumeWeight(1f));
        }
        else if (timeOfDay <= transitionLength && !isDay)
        {
            isDay = true;
            StartCoroutine(TransitionVolumeWeight(0f));
        }
    }

    private IEnumerator TransitionVolumeWeight(float targetWeight)
    {
        float elapsed = 0f;
        float startWeight = globalVolume.weight;
        while (elapsed < transitionLength)
        {
            globalVolume.weight = Mathf.Lerp(startWeight, targetWeight, elapsed / transitionLength);
            elapsed += Time.deltaTime;
            yield return null;
        }
        globalVolume.weight = targetWeight;
    }
}
