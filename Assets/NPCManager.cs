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
    public GameObject Golem;
    public GameObject Bomber;
    public GameObject Container;
    public GameObject ElementalContainer;
    public GameObject NPCContainer;

   

    private List<Vector3Int> availableLocations;

    [Header("Normal Monsters")]
    public List<GameObject> enemyList; // list of enemies to spawn
    public int maxSpawnCount = 140;
    private int currentSpawnCount = 0;

    [Header("Elemetal Monsters")]
    public List<GameObject> elementalList; // list of enemies to spawn
    public int elementalMaxSpawnCount = 6;
    private int elementalCurrentSpawnCount = 0;

    [Header("Fly Totem")]
    public List<GameObject> flyTotemList; // list of enemies to spawn
    public int flyMax = 6;
    private int flyCurrent = 0;

    [Header("Slime Totem")]
    public List<GameObject> slimeTotemList; // list of enemies to spawn
    public int slimeMax = 6;
    private int slimeCurrent = 0;

    public CombatManager combat;
    private void Awake()
    {
        cellular = GetComponent<Cellular>();
    }

    private void Start()
    {
        combat = FindObjectOfType<CombatManager>();
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

        if(elementalCurrentSpawnCount < elementalMaxSpawnCount && availableLocations.Count > 0)
        {
            // Get a random location from the list of available locations
            int randomIndex = Random.Range(0, availableLocations.Count);
            Vector3Int location = availableLocations[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(location) + new Vector3(0.5f, 0.5f, 0f);

            // Get a random enemy from the list of enemies
            int enemyIndex = Random.Range(0, elementalList.Count);
            GameObject enemy = Instantiate(elementalList[enemyIndex], worldPosition, Quaternion.identity, ElementalContainer.transform);

            availableLocations.RemoveAt(randomIndex);

            //Debug.Log("Elemental Enemies: " + elementalCurrentSpawnCount);

            elementalCurrentSpawnCount++;
        }


        if (flyCurrent < flyMax && availableLocations.Count > 0)
        {
            // Get a random location from the list of available locations
            int randomIndex = Random.Range(0, availableLocations.Count);
            Vector3Int location = availableLocations[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(location) + new Vector3(0.5f, 0.5f, 0f);
            // Get a random enemy from the list of enemies
            int enemyIndex = Random.Range(0, enemyList.Count);
            GameObject enemy = Instantiate(flyTotemList[0], worldPosition, Quaternion.identity, Container.transform);
            // Remove the location from the list of available locations
            availableLocations.RemoveAt(randomIndex);
            // Increment the spawn count
            flyCurrent++;
        }

        if (slimeCurrent < slimeMax && availableLocations.Count > 0)
        {
            // Get a random location from the list of available locations
            int randomIndex = Random.Range(0, availableLocations.Count);
            Vector3Int location = availableLocations[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(location) + new Vector3(0.5f, 0.5f, 0f);
            // Get a random enemy from the list of enemies
            int enemyIndex = Random.Range(0, enemyList.Count);
            GameObject enemy = Instantiate(slimeTotemList[0], worldPosition, Quaternion.identity, Container.transform);
            // Remove the location from the list of available locations
            availableLocations.RemoveAt(randomIndex);
            // Increment the spawn count
            slimeCurrent++;
        }
    }

    public void OnFlyDestroyed()
    {
        flyCurrent--;
    }

    public void OnSlimeDestroyed()
    {
        slimeCurrent--;
    }
    public void OnEnemyDestroyed()
    {
       // combat.KillEnemy();
        currentSpawnCount--;
        //Debug.Log("Enemy Alive: " + currentSpawnCount);
    }

    public void OnElementalDestroyed()
    {
        elementalCurrentSpawnCount--;
        //Debug.Log("Enemy Alive: " + elementalCurrentSpawnCount);
    }



    public void SpawnBomber()
    {

        //Bomber 
        int bomberSpawn = 0;
        while (bomberSpawn < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(Bomber, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            bomberSpawn++;
        }
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

            bool isNearWater = false;
            int distance = 2; // Adjust the distance here (e.g., 1 for a 3x3 square, 2 for a 5x5 square)

            for (int x = tilePosition.x - distance; x <= tilePosition.x + distance; x++)
            {
                for (int y = tilePosition.y - distance; y <= tilePosition.y + distance; y++)
                {
                    if (cellular.waterTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    {
                        isNearWater = true;
                        break;
                    }
                }
                if (isNearWater)
                {
                    break;
                }
            }

            // Skip this tile if it's near water
            if (isNearWater)
            {
                continue;
            }

            Instantiate(blackSmith, worldPosition, Quaternion.identity, NPCContainer.transform);
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

            // Check water distance
            bool isNearWater = false;
            int distance = 2; // Adjust the distance here (e.g., 1 for a 3x3 square, 2 for a 5x5 square)

            for (int x = tilePosition.x - distance; x <= tilePosition.x + distance; x++)
            {
                for (int y = tilePosition.y - distance; y <= tilePosition.y + distance; y++)
                {
                    if (cellular.waterTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    {
                        isNearWater = true;
                        break;
                    }
                }
                if (isNearWater)
                {
                    break;
                }
            }

            // Skip this tile if it's near water
            if (isNearWater)
            {
                continue;
            }

            Instantiate(Witch, worldPosition, Quaternion.identity, NPCContainer.transform);
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

            // Check water distance
            bool isNearWater = false;
            int distance = 2; // Adjust the distance here (e.g., 1 for a 3x3 square, 2 for a 5x5 square)

            for (int x = tilePosition.x - distance; x <= tilePosition.x + distance; x++)
            {
                for (int y = tilePosition.y - distance; y <= tilePosition.y + distance; y++)
                {
                    if (cellular.waterTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    {
                        isNearWater = true;
                        break;
                    }
                }
                if (isNearWater)
                {
                    break;
                }
            }

            // Skip this tile if it's near water
            if (isNearWater)
            {
                continue;
            }

            Instantiate(Crow, worldPosition, Quaternion.identity, NPCContainer.transform);
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
            Instantiate(Merchant, worldPosition, Quaternion.identity, NPCContainer.transform);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            merchantspawn++;
        }
    }

    public void SpawnGolem()
    {
        int golemspawn = 0;
        while (golemspawn < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(Golem, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            golemspawn++;
        }
    }


    public int dogspawns = 0;
    public Vector3 dogoTotemSavedPos;
    public void SpawnDogo()
    {

       
        while (dogspawns < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            dogoTotemSavedPos = worldPosition;
            Instantiate(Dogo, dogoTotemSavedPos, Quaternion.identity);
            
            cellular.groundTilePositions.RemoveAt(randomIndex);
            dogspawns++;
        }
    }

    public void SpawnRuin1()
    {

        int ruinSpawn = 0;
        while (ruinSpawn < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile

            bool isNearWater = false;
            int distance = 2; // Adjust the distance here (e.g., 1 for a 3x3 square, 2 for a 5x5 square)

            for (int x = tilePosition.x - distance; x <= tilePosition.x + distance; x++)
            {
                for (int y = tilePosition.y - distance; y <= tilePosition.y + distance; y++)
                {
                    if (cellular.waterTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    {
                        isNearWater = true;
                        break;
                    }
                }
                if (isNearWater)
                {
                    break;
                }
            }

            // Skip this tile if it's near water
            if (isNearWater || tilePosition.y < 20 || tilePosition.y > 40)
            {
                continue;
            }

            Instantiate(bonFire, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            ruinSpawn++;
        }

    }
    public void SpawnRuin2()
    {

        int ruinSpawn = 0;
        while (ruinSpawn < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile

            bool isNearWater = false;
            int distance = 2; // Adjust the distance here (e.g., 1 for a 3x3 square, 2 for a 5x5 square)

            for (int x = tilePosition.x - distance; x <= tilePosition.x + distance; x++)
            {
                for (int y = tilePosition.y - distance; y <= tilePosition.y + distance; y++)
                {
                    if (cellular.waterTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    {
                        isNearWater = true;
                        break;
                    }
                }
                if (isNearWater)
                {
                    break;
                }
            }

            // Skip this tile if it's near water
            if (isNearWater || tilePosition.y < 60 || tilePosition.y > 90)
            {
                continue;
            }

            Instantiate(bonFire, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            ruinSpawn++;
        }

    }

    public void SpawnRuin3()
    {

        int ruinSpawn = 0;
        while (ruinSpawn < 1 && cellular.groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, cellular.groundTilePositions.Count);
            Vector3Int tilePosition = cellular.groundTilePositions[randomIndex];
            Vector3 worldPosition = cellular.tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile

            bool isNearWater = false;
            int distance = 2; // Adjust the distance here (e.g., 1 for a 3x3 square, 2 for a 5x5 square)

            for (int x = tilePosition.x - distance; x <= tilePosition.x + distance; x++)
            {
                for (int y = tilePosition.y - distance; y <= tilePosition.y + distance; y++)
                {
                    if (cellular.waterTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    {
                        isNearWater = true;
                        break;
                    }
                }
                if (isNearWater)
                {
                    break;
                }
            }

            // Skip this tile if it's near water
            if (isNearWater || tilePosition.y < 110 || tilePosition.y > 140)
            {
                continue;
            }

            Instantiate(bonFire, worldPosition, Quaternion.identity);
            cellular.groundTilePositions.RemoveAt(randomIndex);
            ruinSpawn++;
        }

    }




}
