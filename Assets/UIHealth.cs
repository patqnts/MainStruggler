using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public Image emptyHeartPrefab;
    public Image filledHeartPrefab;
    public Transform heartsContainer;
    private int maxHearts;
    private Image[] hearts;
    public Movement player;

   

    void Start()
    {

        maxHearts = Mathf.CeilToInt(player.maxHealth / 1f);

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
        player.maxHealth++;
        player._health++;
        SetMaxHearts(player.maxHealth);

    }
    public void SetMaxHearts(float maxHealth)
    {
        maxHearts = Mathf.CeilToInt(maxHealth / 1f); // assuming each heart represents 2 health points
                                                            // delete existing heart images if the number of hearts is less than maxHearts
        while (hearts.Length < maxHearts)
        {
            Instantiate(emptyHeartPrefab, heartsContainer);
            hearts = heartsContainer.GetComponentsInChildren<Image>();
        }
        // remove excess heart images if the number of hearts is greater than maxHearts
        while (hearts.Length > maxHearts)
        {
            Destroy(hearts[hearts.Length - 1].gameObject);
            hearts = heartsContainer.GetComponentsInChildren<Image>();
        }
    }

}