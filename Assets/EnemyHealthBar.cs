using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Slider delayedSlider; // Reference to the duplicate slider
    [SerializeField] private float smoothTime = 0.1f;

    private float targetValue;
    private float delayedTargetValue;
    private float delayTime = 2f; // Delay time in seconds

    private float currentDelay;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        targetValue = currentHealth / maxHealth;

        // Update the delayed target value after the delay time
        currentDelay = delayTime;
        StartCoroutine(DelayedUpdateHealthBar());
    }

    private IEnumerator DelayedUpdateHealthBar()
    {
        yield return new WaitForSeconds(delayTime);

        delayedTargetValue = targetValue;
    }

    private void LateUpdate()
    {
        // Smoothly update the original slider
        slider.value = Mathf.SmoothDamp(slider.value, targetValue, ref velocity, smoothTime);

        // Smoothly update the delayed slider
        delayedSlider.value = Mathf.SmoothDamp(delayedSlider.value, delayedTargetValue, ref velocity, smoothTime);
    }

    private float velocity;

    void Start()
    {
        targetValue = slider.value;
        delayedTargetValue = delayedSlider.value;
    }
}
