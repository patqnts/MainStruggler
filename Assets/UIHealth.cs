using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public Movement player;
    public Image heartPrefab;
    public Transform heartsContainer;
    public int maxHearts = 10;

    private Image[] hearts;

    void Start()
    {
        // Create heart images based on maxHearts
        hearts = new Image[maxHearts];
        for (int i = 0; i < maxHearts; i++)
        {
            hearts[i] = Instantiate(heartPrefab, heartsContainer);
        }
    }

    void Update()
    {
        // Update heart images based on player's health
        float fillAmount = Mathf.Round(player.Health) / maxHearts;
        for (int i = 0; i < maxHearts; i++)
        {
            hearts[i].fillAmount = Mathf.Clamp01(fillAmount - i);
        }
    }

    public void AddHeart()
    {
        // Add a new heart image to the UI
        Instantiate(heartPrefab, heartsContainer);
        maxHearts++;
    }
}
