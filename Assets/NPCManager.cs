using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public Cellular cellular;

    public GameObject SlimeQueen;
    public GameObject blackSmith;
    public GameObject Merchant;
    public GameObject bonFire;
    public GameObject Witch;
    public GameObject Crow;
    public GameObject Dogo;
    public GameObject Container;
    private List<Vector3Int> availableLocations;
    public List<GameObject> enemyList; // list of enemies to spawn
    public int maxSpawnCount;
    private int currentSpawnCount = 0;

    private void Awake()
    {
        cellular = GetComponent<Cellular>();
    }

    private void Start()
    {
        // Get all available locations on the map
        availableLocations = new List<Vector3Int>();
        foreach (var position in cellular.tilemap.cellBounds.allPositionsWithin)
        {
            if (cellular.tilemap.HasTile(position))
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(cellular.tilemap.CellToWorld(position) + new Vector3(0.5f, 0.5f, 0f), 0.1f);
                if (colliders.Length == 0)
                {
                    availableLocations.Add(position);
                }
            }
        }

        // Spawn enemies
    }

    private void Update()
    {
        if (currentSpawnCount < maxSpawnCount && availableLocations.Count > 0)
        {
            // Get a random location from the list of available locations
            int randomIndex = Random.Range(0, availableLocations.Count);
            Vector3Int location = availableLocations[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(location) + new Vector3(0.5f, 0.5f, 0f);

            // Get a random enemy from the list of enemies
            int enemyIndex = Random.Range(0, enemyList.Count);
            GameObject enemy = Instantiate(enemyList[enemyIndex], worldPosition, Quaternion.identity,Container.transform);

            // Remove the location from the list of available locations
            availableLocations.RemoveAt(randomIndex);

            // Increment the spawn count
            currentSpawnCount++;
            
        }
    }
    public void OnEnemyDestroyed()
    {
        currentSpawnCount--;
        Debug.Log("Enemy Alive: " + currentSpawnCount);
    }





    public void SpawnQueen()
    {

        //SLIME QUEEN
        int slimequeenSpawn = 0;
        while (slimequeenSpawn < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(SlimeQueen, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            slimequeenSpawn++;
        }
    }
    public void SpawnSmith()
    {

        int blacksmithSpawn = 0;
        while (blacksmithSpawn < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(blackSmith, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            blacksmithSpawn++;
        }
    }
    
    public void SpawnWitch()
    {
        //WITCH
        int witchspawn = 0;
        while (witchspawn < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(Witch, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            witchspawn++;
        }
    }
    public void SpawnCrow()
    {
        //WITCH
        int crowspawn = 0;
        while (crowspawn < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(Crow, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            crowspawn++;
        }
    }
    public void SpawnMerchant()
    {
        int merchantspawn = 0;
        while (merchantspawn < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(Merchant, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            merchantspawn++;
        }
    }

    public void SpawnRuin()
    {
        
        int ruinspawn = 0;
        while (ruinspawn < 3 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(bonFire, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            ruinspawn++;
        }
    }

    public void SpawnDogo()
    {

        int dogspawn = 0;
        while (dogspawn < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(Dogo, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            dogspawn++;
        }
    }

}