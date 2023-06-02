using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class DayNightCycles : MonoBehaviour
{
    [SerializeField] private float dayLength = 300f; // Day length in seconds
    [SerializeField] private float nightLength = 300f; // Night length in seconds
    [SerializeField] public float transitionLength = 10f; // Transition length in seconds
    private Light2D localLight2D; // Local Light2D component to control the look of the game
   
    private Color dayColor = Color.white; // Color for day
    private Color nightColor = new Color(0.365f, 0.537f, 1f); // Color for night
    private float dayIntensity = 1f; // Intensity for day
    private float nightIntensity = 0.5f; // Intensity for night

    public float timeOfDay = 0f; // Current time of day in seconds
    public bool isNight; // Whether it's currently night time

    private void Start()
    {
        // Get the Local Light2D component
        localLight2D = GetComponent<Light2D>();

        // Set the initial color and intensity of the Local Light2D component to represent day time
        LoadSystem load = FindObjectOfType<LoadSystem>();
         
    }
    public void InitializeDayNightCycles()
    {
        // Get the Local Light2D component
        localLight2D = GetComponent<Light2D>();

        // Set the initial color and intensity of the Local Light2D component to represent day time
        if (!isNight)
        {
            localLight2D.color = dayColor;
            localLight2D.intensity = dayIntensity;

            Debug.Log("is day: IS NIGHT IS " + isNight);
        }
        else
        {
            localLight2D.color = nightColor;
            localLight2D.intensity = nightIntensity;
            Debug.Log("is night IS NIGHT IS " + isNight);
        }
    }


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
        float startIntensity = localLight2D.intensity;
        Color startColor = localLight2D.color;

        float endIntensity = nightIntensity;
        Color endColor = nightColor;

        Color middleColor = new Color(1f, 0.7608f, 0.8118f); // Hexadecimal FFC2CF
        float middleIntensity = 0.7f;

        while (t < transitionLength)
        {
            t += Time.deltaTime;
            float normalizedTime = t / transitionLength;

            if (normalizedTime < 0.5f)
            {
                localLight2D.intensity = Mathf.Lerp(startIntensity, middleIntensity, normalizedTime * 2f);
                localLight2D.color = Color.Lerp(startColor, middleColor, normalizedTime * 2f);
            }
            else
            {
                localLight2D.intensity = Mathf.Lerp(middleIntensity, endIntensity, (normalizedTime - 0.5f) * 2f);
                localLight2D.color = Color.Lerp(middleColor, endColor, (normalizedTime - 0.5f) * 2f);
            }

            
            yield return null;
        }

        localLight2D.intensity = endIntensity;
        localLight2D.color = endColor;
    }

    


    private IEnumerator TransitionToDay()
    {
        float t = 0f;
        float startIntensity = localLight2D.intensity;
        Color startColor = localLight2D.color;

        float endIntensity = dayIntensity;
        Color endColor = dayColor;

        while (t < transitionLength)
        {
            t += Time.deltaTime;
            float normalizedTime = t / transitionLength;
            localLight2D.intensity = Mathf.Lerp(startIntensity, endIntensity, normalizedTime);
            localLight2D.color = Color.Lerp(startColor, endColor, normalizedTime);
           
            yield return null;
        }

        localLight2D.intensity = endIntensity;
        localLight2D.color = endColor;
    }
}
