using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Cellular : MonoBehaviour
{
   public LoadSystem loadSystem;

    [Header("1ST TERRAIN")]
  
    public int mapWidth;
    public int mapHeight;
    public int smoothnessIterations = 5;
    public int fillPercentage = 45;
    public Tilemap tilemap;
    public Tilemap flowergrasstilemap;
    public NPCManager npcManager;
    //COLLIDER TILEMAP
    public Tilemap waterTilemap;
    //ruin prefab

    public GameObject bottle;
   

    public TileBase[] grassflowers;
    public TileBase[] oldgrass;
    //----------------------------//
    public TileBase[] tileset;
    private int[,] map;
    public List<Vector3Int> groundTilePositions = new List<Vector3Int>();
    public GameObject player;
    public GameObject camera;
    public GameObject[] treePrefabs;
    
    public string text;

    public int seedCode = 0;
    public int seedCodex;

    public bool newLoad = false;
    private void Update()
    {
        //PlayerPrefs.SetInt("seedCode", seedCode);
       // PlayerPrefs.Save();

    }
    private void Start()
    {
        
        loadSystem = FindObjectOfType<LoadSystem>();

        text = loadSystem.passedText;

        loadSystem.LoadPlayer(text);


        //   Debug.Log(loadSystem.selectedProfileId);
        // Reset seedCode in PlayerPrefs
        PlayerPrefs.DeleteKey("seedCode");

        // GenerateMap will use the new seed value
        seedCode = loadSystem.lastSeedCode;
        seedCodex = seedCode;

        // Save the new seed value
        PlayerPrefs.SetInt("seedCode", seedCode);
        PlayerPrefs.Save();

        // Generate the map
        GenerateMap();
        camera.transform.position = player.transform.position;

        npcManager.SpawnQueen();
        npcManager.SpawnCrow();
        npcManager.SpawnDogo();
        npcManager.SpawnMerchant();
        npcManager.SpawnSmith();

        npcManager.SpawnRuin1();
        npcManager.SpawnRuin2();
        npcManager.SpawnRuin3();


        npcManager.SpawnWitch();
        npcManager.SpawnGolem();
        npcManager.SpawnBomber();


       
       // loadSystem.LoadPlayer(text);
        if (newLoad)
        {
            RuinSavePoint.PlayerDied();
            Bottle();
            //loadSystem.LoadPlayer(text);
        }
        else
        {
            //loadSystem.LoadPlayer(text);
            Debug.Log("wala lang");
        }
        
    }
    public void Bottle()
    {
        // Get the player's position.
        Vector3 playerPosition = player.transform.position;

        // Generate a random position within 6-8 units of the player.
        Vector3 randomPosition = playerPosition + new Vector3(
            Random.Range(-6, 8),
            Random.Range(-6, 8),
            0f
        );

        // Check if the random position is on a ground tile.
        bool isOnGroundTile = false;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (tilemap.GetTile(new Vector3Int((int)randomPosition.x + x, (int)randomPosition.y + y, 0)) != null)
                {
                    isOnGroundTile = true;
                    break;
                }
            }
            if (isOnGroundTile)
            {
                break;
            }
        }

        // If the random position is on a ground tile, instantiate the prefab.
        if (isOnGroundTile)
        {
            Instantiate(bottle, randomPosition, Quaternion.identity);
        }


    }

    private void GenerateMapUsingSeed()
    {
        Random.InitState(seedCode); // Initialize the random number generator with the seed code

        map = new int[mapWidth, mapHeight];
        FillMapRandomly();

        Vector2 center = new Vector2(mapWidth / 2, mapHeight / 2);
        float maxDistance = Mathf.Min(center.x - 9, center.y - 9); // Subtract tiles from center coordinates

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (Vector2.Distance(new Vector2(x, y), center) <= maxDistance)
                {
                    map[x, y] = Random.Range(0, 100) < fillPercentage ? 1 : 0;
                }
                else
                {
                    map[x, y] = 0;
                }
            }
        }

        for (int i = 0; i < smoothnessIterations; i++)
        {
            SmoothMap();
        }
    }





    private void GenerateMap()
    {
        if (seedCode == 0)
        {
           
            seedCode = Random.Range(1, int.MaxValue); // Generate a random seed code
            GenerateMapUsingSeed();
            seedCodex = seedCode;
            Debug.Log("SEED = 0");
            newLoad = true;
            //RuinSavePoint.PlayerDied();

        }
        else
        {
            
            Random.InitState(seedCode);
            GenerateMapUsingSeed();
            Debug.Log("SEED = " + seedCode);
        }
        SmoothMap();

        for (int x = 0; x < mapWidth; x++)
        {
            int tilesetground = 0;
            int water = 1;
            int transition = 2;
            for (int y = 0; y < mapHeight; y++)
            {
                if (map[x, y] == 1)
                {

                    tilemap.SetTile(new Vector3Int(x, y, 0), tileset[tilesetground]); // replace with the tile you want to use for ground
                    groundTilePositions.Add(new Vector3Int(x, y, 0));

                }
                else if (x > mapWidth)
                {
                    waterTilemap.SetTile(new Vector3Int(x, y, 0), tileset[transition]); // replace with the tile you want to use for water
                                                                                        // Add collider to water tile
                    waterTilemap.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.None);
                    waterTilemap.SetColliderType(new Vector3Int(x, y, 0), Tile.ColliderType.Grid);


                }
                else
                {
                 waterTilemap.SetTile(new Vector3Int(x, y, 0), tileset[water]); // replace with the tile you want to use for water

                // Add collider to water tile
                 waterTilemap.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.None);
                 waterTilemap.SetColliderType(new Vector3Int(x, y, 0), Tile.ColliderType.Grid);

                }

                // Randomly add grass tiles on ground tiles
                if (map[x, y] == 1 && waterTilemap.GetTile(new Vector3Int(x, y, 0)) == null)
                {
                    bool isCenter = true;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int nx = x + i;
                            int ny = y + j;
                            if (nx < 0 || ny < 0 || nx >= mapWidth || ny >= mapHeight || map[nx, ny] != map[x, y])
                            {
                                isCenter = false;
                                break;
                            }
                        }
                        if (!isCenter)
                        {
                            break;
                        }
                    }
                    if (isCenter && Random.Range(0, 100) < 90)
                    {
                        flowergrasstilemap.SetTile(new Vector3Int(x, y, 0), oldgrass[Random.Range(0, 1)]);
                    }
                    else
                    {
                        // 10% chance to spawn an old grass tile
                        if (Random.Range(0, 100) < 10)
                        {
                            flowergrasstilemap.SetTile(new Vector3Int(x, y, 0), tileset[transition]);
                        }
                    }
                }


                if (map[x, y] == 1 && waterTilemap.GetTile(new Vector3Int(x, y, 0)) == null)
                {
                    bool isCenter = true;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int nx = x + i;
                            int ny = y + j;
                            if (nx < 0 || ny < 0 || nx >= mapWidth || ny >= mapHeight || map[nx, ny] != map[x, y])
                            {
                                isCenter = false;
                                break;
                            }
                        }
                        if (!isCenter)
                        {
                            break;
                        }
                    }
                    if (isCenter && Random.Range(0, 100) < 90)
                    {
                        flowergrasstilemap.SetTile(new Vector3Int(x, y, 0), grassflowers[Random.Range(0, 1)]);
                    }
                }
            }

        }
        float treePrefabSize = 2f;
        // Add trees to the ground tiles
        int numTrees = Mathf.RoundToInt(groundTilePositions.Count / 3); // 10% of the ground tiles will have trees
        for (int i = 0; i < numTrees; i++)
        {
            // Find a random ground tile
            int randomIndex = Random.Range(0, groundTilePositions.Count);
            Vector3Int tilePosition = groundTilePositions[randomIndex];

            // Check if the position is near water
            bool isNearWater = false;
            for (int x = tilePosition.x - 1; x <= tilePosition.x + 1; x++)
            {
                for (int y = tilePosition.y - 1; y <= tilePosition.y + 1; y++)
                {
                    if (waterTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
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

            Vector3 treePosition = tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the tree on the tile

            // Check if the position is clear of other objects
            Collider2D[] overlaps = Physics2D.OverlapCircleAll(treePosition, treePrefabSize);
            bool canPlaceTree = true;
            foreach (Collider2D overlap in overlaps)
            {
                if (overlap.gameObject.CompareTag("Rock") || overlap.gameObject.CompareTag("Tree"))
                {
                    canPlaceTree = false;
                    break;
                }
            }

            if (canPlaceTree)
            {
                // Instantiate the selected tree prefab
                int randomTreeIndex = Random.Range(0, treePrefabs.Length);
                GameObject tree = Instantiate(treePrefabs[randomTreeIndex], treePosition, Quaternion.identity);

                // Set the tree as a child of the Cellular object
                tree.transform.SetParent(transform);

                // Remove the ground tile from the list of available positions so that another tree isn't placed on the same tile
                groundTilePositions.RemoveAt(randomIndex);
            }
            else
            {
                // Try again if the position is not clear
                continue;
            }
        }

    }

    private void FillMapRandomly()
    {
        Vector2 center = new Vector2(mapWidth / 2, mapHeight / 2);
        float maxDistance = Mathf.Min(center.x - 9, center.y - 9); // Subtract  tiles from center coordinates

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (Vector2.Distance(new Vector2(x, y), center) <= maxDistance)
                {
                    map[x, y] = Random.Range(0, 100) < fillPercentage ? 1 : 0;
                }
                else
                {
                    map[x, y] = 0;
                }
            }
        }
    }



    private void SmoothMap()
    {
        for (int i = 0; i < smoothnessIterations; i++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    int neighborWallTiles = GetSurroundingWallCount(x, y);

                    if (neighborWallTiles > 4)
                        map[x, y] = 1;
                    else if (neighborWallTiles < 4)
                        map[x, y] = 0;
                }
            }
        }
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;

        for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
        {
            for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
            {
                if (neighborX >= 0 && neighborX < mapWidth && neighborY >= 0 && neighborY < mapHeight)
                {
                    if (neighborX != gridX || neighborY != gridY)
                    {
                        wallCount += map[neighborX, neighborY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }
}
