using UnityEngine;

[CreateAssetMenu(fileName = "New Health Data", menuName = "Player/Health Data")]
public class HealthData : ScriptableObject
{
    public int maxHealth = 3;
    public int currentHealth = 3;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthChanged?.Invoke();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthChanged?.Invoke();
    }

    public delegate void HealthChangedEventHandler();
    public event HealthChangedEventHandler HealthChanged;
}
