using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class TerrainTest : MonoBehaviour
{
    [Header("1ST TERRAIN")]
    public int mapWidth;
    public int mapHeight;
    public int fillPercentage = 45;
    public Tilemap tilemap;
    public Tilemap treeTilemap;
    public TileBase[] tileset;
    public GameObject[] treePrefabs;

    private int[,] map;
    private List<Vector3Int> groundTilePositions = new List<Vector3Int>();

    private void Start()
    {
        GenerateMap();
        PlaceTrees();
    }

    private void GenerateMap()
    {
        map = new int[mapWidth, mapHeight];
        FillMapRandomly();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileset[0]); // replace with the tile you want to use for ground
                    groundTilePositions.Add(new Vector3Int(x, y, 0));
                }
            }
        }
    }

    private void FillMapRandomly()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                map[x, y] = Random.Range(0, 100) < fillPercentage ? 1 : 0;
            }
        }
    }

    private void PlaceTrees()
    {
        foreach (Vector3Int tilePosition in groundTilePositions)
        {
            if (Random.Range(0, 100) < 10) // 10% chance of placing a tree on each ground tile
            {
                Vector3 treePosition = tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f); // add offset to center the tree on the tile

                // Check if the position is clear of other objects
                bool canPlaceTree = !Physics2D.OverlapCircle(treePosition, 1f);

                if (canPlaceTree)
                {
                    // Instantiate the selected tree prefab
                    int randomTreeIndex = Random.Range(0, treePrefabs.Length);
                    GameObject tree = Instantiate(treePrefabs[randomTreeIndex], treePosition, Quaternion.identity);

                    // Set the tree as a child of the Cellular object
                    tree.transform.SetParent(transform);

                    // Set the tree tile on the tree tilemap
                    treeTilemap.SetTile(treeTilemap.WorldToCell(treePosition), tileset[1]); // replace with the tile you want to use for trees
                }
            }
        }
    }
}
