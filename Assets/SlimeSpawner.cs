using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxAliveEnemies = 1;
    public float spawnDelay = 5.0f;

    private static int currentAliveEnemies = 0;
    private float lastSpawnTime = 0f;

    private void Update()
    {
        // Check if there are any alive enemies
        if (currentAliveEnemies == 0)
        {
            // Check if it's time to spawn a new enemy
            if (Time.time - lastSpawnTime >= spawnDelay)
            {
                // Spawn a new enemy
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                currentAliveEnemies++;
                lastSpawnTime = Time.time;
            }
        }
    }

    public void EnemyDied()
    {
        // Decrement the alive enemy counter
        currentAliveEnemies = Mathf.Max(0, currentAliveEnemies - 1);
    }
}
