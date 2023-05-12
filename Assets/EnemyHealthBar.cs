using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float smoothTime = 0.1f;

    private float targetValue;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        targetValue = currentHealth / maxHealth;
    }

    private void LateUpdate()
    {
        slider.value = Mathf.SmoothDamp(slider.value, targetValue, ref velocity, smoothTime);
    }

    private float velocity;

    void Start()
    {
        targetValue = slider.value;
    }
}
