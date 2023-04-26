using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float dayLength = 300f; // Day length in seconds
    [SerializeField] private float nightLength = 300f; // Night length in seconds
    [SerializeField] private float transitionLength = 10f; // Transition length in seconds
    [SerializeField] private Volume globalVolume; // Global volume to control the look of the game

    private float timeOfDay = 0f; // Current time of day in seconds
    private bool isNight = false; // Whether it's currently night time

    private void Update()
    {
        // Update the time of day based on the real time
        timeOfDay += Time.deltaTime;

        // Check if it's time to transition to night time
        if (!isNight && timeOfDay >= dayLength)
        {
            isNight = true;
            timeOfDay = 0f;
            StartCoroutine(TransitionToNight());
        }

        // Check if it's time to transition back to day time
        if (isNight && timeOfDay >= nightLength)
        {
            isNight = false;
            timeOfDay = 0f;
            StartCoroutine(TransitionToDay());
        }
    }

    private IEnumerator TransitionToNight()
    {
        float t = 0f;
        float startWeight = globalVolume.weight;
        float endWeight = 1f;
        while (t < transitionLength)
        {
            t += Time.deltaTime;
            float normalizedTime = t / transitionLength;
            globalVolume.weight = Mathf.Lerp(startWeight, endWeight, normalizedTime);
            yield return null;
        }
        globalVolume.weight = endWeight;
    }

    private IEnumerator TransitionToDay()
    {
        float t = 0f;
        float startWeight = globalVolume.weight;
        float endWeight = 0f;
        while (t < transitionLength)
        {
            t += Time.deltaTime;
            float normalizedTime = t / transitionLength;
            globalVolume.weight = Mathf.Lerp(startWeight, endWeight, normalizedTime);
            yield return null;
        }
        globalVolume.weight = endWeight;
    }
}
