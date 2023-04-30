using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Cellular : MonoBehaviour
{

    [Header("1ST TERRAIN")]
  
    public int mapWidth;
    public int mapHeight;
    public int smoothnessIterations = 5;
    public int fillPercentage = 45;
    public Tilemap tilemap;

   
    

    //COLLIDER TILEMAP
    public Tilemap waterTilemap;
    //ruin prefab
    public GameObject blackSmith;
    public GameObject Merchant;
    public GameObject bonFire;
    public GameObject Witch;
    public int numBlackSmith = 1;
    public int numBonfire = 3;
    public int numMerchant = 3;
    public int numWitch = 1;

    public TileBase[] grassflowers;
    public TileBase[] oldgrass;
    //----------------------------//
    public TileBase[] tileset;
    private int[,] map;
    private List<Vector3Int> groundTilePositions = new List<Vector3Int>();
    public GameObject player;
    public GameObject camera;
    public GameObject[] treePrefabs;
    public Tile water;


    public int seedCode = 0;

    private void Update()
    {
        PlayerPrefs.SetInt("seedCode", seedCode);
        PlayerPrefs.Save();

    }
    private void Start()
    {

        seedCode = PlayerPrefs.GetInt("seedCode", 0);


        GenerateMap();

        if (groundTilePositions.Count > 0) // spawn on ground
        {
            int randomIndex = Random.Range(0, groundTilePositions.Count);
            Vector3 playerPosition = tilemap.CellToWorld(groundTilePositions[randomIndex]);

            // Check if the player is spawning in water
            Vector3Int playerCell = tilemap.WorldToCell(playerPosition);
            TileBase tile = waterTilemap.GetTile(playerCell);
            if (tile != null)
            {
                // Set the player's position to the nearest ground tile
                float closestDistance = float.MaxValue;
                Vector3 closestPosition = playerPosition;
                foreach (Vector3Int groundTilePosition in groundTilePositions)
                {
                    Vector3 groundTileWorldPosition = tilemap.CellToWorld(groundTilePosition);
                    float distance = Vector3.Distance(playerPosition, groundTileWorldPosition);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPosition = groundTileWorldPosition;
                    }
                }

                player.transform.position = closestPosition;
            }
            else
            {
                player.transform.position = playerPosition;
            }
        }

        camera.transform.position = player.transform.position;

        int blacksmithSpawn = 0;
        while (blacksmithSpawn < numBlackSmith && groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, groundTilePositions.Count);
            Vector3Int tilePosition = groundTilePositions[randomIndex];
            Vector3 worldPosition = tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(blackSmith, worldPosition, Quaternion.identity);
            groundTilePositions.RemoveAt(randomIndex);
            blacksmithSpawn++;
        }
        int merchantSpawn = 0;
        while (merchantSpawn < numMerchant && groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, groundTilePositions.Count);
            Vector3Int tilePosition = groundTilePositions[randomIndex];
            Vector3 worldPosition = tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(Merchant, worldPosition, Quaternion.identity);
            groundTilePositions.RemoveAt(randomIndex);
            merchantSpawn++;
        }
        int bonfire = 0;
        while(bonfire < numBonfire && groundTilePositions.Count > 0)
                {
            int randomIndex = Random.Range(0, groundTilePositions.Count);
            Vector3Int tilePosition = groundTilePositions[randomIndex];
            Vector3 worldPosition = tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(bonFire, worldPosition, Quaternion.identity);
            groundTilePositions.RemoveAt(randomIndex);
            bonfire++;
        }
        int witchspawn = 0;
        while (witchspawn < numWitch && groundTilePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, groundTilePositions.Count);
            Vector3Int tilePosition = groundTilePositions[randomIndex];
            Vector3 worldPosition = tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the ruin on the tile
            Instantiate(Witch, worldPosition, Quaternion.identity);
            groundTilePositions.RemoveAt(randomIndex);
            witchspawn++;
        }
    }
   
    private void GenerateMapUsingSeed()
    {
       
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // Generate a random number using the stored seed code
                float randomValue = Random.Range(0f, 1f);
                if (randomValue < fillPercentage / 100f)
                {
                    map[x, y] = 1;
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
            map = new int[mapWidth, mapHeight];
            FillMapRandomly();
        }
        else
        {
            Random.InitState(seedCode);
            GenerateMapUsingSeed();
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
                if (map[x, y] == 1 && waterTilemap.GetTile(new Vector3Int(x, y, 0)) == null && Random.Range(0, 100) < 70)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 2), oldgrass[Random.Range(0, 1)]); // replace with the tile you want to use for grass
                }

                if (map[x, y] == 1 && waterTilemap.GetTile(new Vector3Int(x, y, 0)) == null && Random.Range(0, 100) < 70)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 2), grassflowers[Random.Range(0,1)]); // replace with the tile you want to use for grass
                }

            }



        }
        // Add trees to the ground tiles
        int numTrees = Mathf.RoundToInt(groundTilePositions.Count / 10); // 10% of the ground tiles will have trees
        for (int i = 0; i < numTrees; i++)
        {
            // Find a random ground tile
            int randomIndex = Random.Range(0, groundTilePositions.Count);
            Vector3 treePosition = tilemap.CellToWorld(groundTilePositions[randomIndex]) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the tree on the tile

            // Instantiate the selected tree prefab
            int randomTreeIndex = Random.Range(0, treePrefabs.Length);
            GameObject tree = Instantiate(treePrefabs[randomTreeIndex], treePosition, Quaternion.identity);

            // Set the tree as a child of the Cellular object
            tree.transform.SetParent(transform);

            // Remove the ground tile from the list of available positions so that another tree isn't placed on the same tile
            groundTilePositions.RemoveAt(randomIndex);
        }



    }




    private void FillMapRandomly()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (x <= 15 || x >= mapWidth - 20 || y <= 20 || y == mapHeight - 20 || x == 1 || x == mapWidth - 20 || y == 1 || y >= mapHeight - 20)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = Random.Range(0, 100) < fillPercentage ? 1 : 0;
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
