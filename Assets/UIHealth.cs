using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
   
    public Image emptyHeartPrefab;
    public Image filledHeartPrefab;
    public Transform heartsContainer;
    public int maxHearts;

    private Image[] hearts;

    void Start()
    {
        // Create heart images based on maxHearts
        hearts = new Image[maxHearts];
        for (int i = 0; i < maxHearts; i++)
        {
            hearts[i] = Instantiate(emptyHeartPrefab, heartsContainer);
        }
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        float fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        int filledHearts = Mathf.CeilToInt(fillAmount * maxHearts);

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < filledHearts)
            {
                hearts[i].sprite = filledHeartPrefab.sprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartPrefab.sprite;
            }
        }
    }



    public void AddHeart()
    {
        // Add a new heart image to the UI
        Instantiate(emptyHeartPrefab, heartsContainer);
        maxHearts++;
    }
}
