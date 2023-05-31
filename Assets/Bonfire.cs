using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bonfire : MonoBehaviour
{
    public Light2D wispLight2D;
    public DayNightCycles cycle;
    private float damage = 5f;
    private float transitionDuration;
    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();
        if (collision.gameObject != null && damageableObject != null)
        {
            if (collision.gameObject.CompareTag("Object"))
            {
                damageableObject.OnHit(damage);
            }
        }
    }

    private void Start()
    {
        cycle = FindObjectOfType<DayNightCycles>();
        if (cycle != null)
            transitionDuration = cycle.transitionLength;
    }

    private void Update()
    {
        if (cycle != null && !isTransitioning)
        {
            float targetIntensity = cycle.isNight ? 1.83f : 0f;
            StartCoroutine(ChangeIntensitySmoothly(wispLight2D, targetIntensity, transitionDuration));
        }
    }

    private IEnumerator ChangeIntensitySmoothly(Light2D light, float targetIntensity, float duration)
    {
        isTransitioning = true;
        float initialIntensity = light.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            light.intensity = Mathf.Lerp(initialIntensity, targetIntensity, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        light.intensity = targetIntensity;
        isTransitioning = false;
    }
}
