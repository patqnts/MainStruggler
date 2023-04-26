using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public Tilemap tilemap;
    public TileBase[] tileset;

  



    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        GenerateMap();
        
    }

    private void GenerateMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float noise = Mathf.PerlinNoise((float)x / 10f, (float)y / 10f);
                TileBase tile = ChooseTileFromSet(noise);
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);

                if (noise > 0.5f && tile == tileset[0])
                {
                    TileBase flowerTile = ChooseFlowerTileFromSet();
                    tilemap.SetTile(new Vector3Int(x, y, 1), flowerTile);
                }
            }
        }
    }

    private TileBase ChooseFlowerTileFromSet()
    {
        return tileset[3]; // replace with the flower tile you want to use
    }

    private TileBase ChooseTileFromSet(float noise)
    {
        if (noise < 0.4f)
        {
            return tileset[0];
        }
        else if (noise < 0.6f)
        {
            return tileset[1];
        }
        else
        {
            return tileset[2];
        }
    }


    

   
}
