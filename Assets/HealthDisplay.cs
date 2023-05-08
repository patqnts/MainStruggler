using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public HealthData healthData;
    public Image[] heartImages;

    private void OnEnable()
    {
        healthData.HealthChanged += UpdateHearts;
        UpdateHearts();
    }

    private void OnDisable()
    {
        healthData.HealthChanged -= UpdateHearts;
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < healthData.currentHealth)
            {
                heartImages[i].sprite = healthData.fullHeartSprite;
            }
            else
            {
                heartImages[i].sprite = healthData.emptyHeartSprite;
            }
        }
    }
}
