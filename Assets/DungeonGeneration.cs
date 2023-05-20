using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGeneration : MonoBehaviour
{
    public int dungeonWidth = 30;               // Width of the dungeon in tiles
    public int dungeonHeight = 30;              // Height of the dungeon in tiles

    public TileBase floorTile;                  // Tile used for the floor
    public TileBase wallTile;                   // Tile used for the walls

    public int minRoomSize = 5;                 // Minimum size of a room
    public int maxRoomSize = 10;                // Maximum size of a room
    public int maxRooms = 20;                   // Maximum number of rooms

    public TileBase pathwayTile;                // Tile used for pathways

    public Tilemap wallTilemap;                 // Reference to the wall Tilemap component
    public Tilemap groundTilemap;               // Reference to the ground Tilemap component
    public Tilemap pathwayTilemap;              // Reference to the pathway Tilemap component

    private List<Room> rooms = new List<Room>(); // List to store the generated rooms

    private class Room
    {
        public int x;
        public int y;
        public int width;
        public int height;

        public Room(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        // Create a 2D array to store the dungeon layout
        int[,] dungeon = new int[dungeonWidth, dungeonHeight];

        // Initialize the dungeon with walls
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                dungeon[x, y] = 1; // Set as a wall tile
            }
        }

        // Generate rooms
        for (int i = 0; i < maxRooms; i++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);
            int roomX = Random.Range(1, dungeonWidth - roomWidth - 1);
            int roomY = Random.Range(1, dungeonHeight - roomHeight - 1);

            Room newRoom = new Room(roomX, roomY, roomWidth, roomHeight);

            // Check if the new room overlaps with existing rooms
            bool isOverlapping = false;
            foreach (Room existingRoom in rooms)
            {
                if (CheckRoomOverlap(newRoom, existingRoom))
                {
                    isOverlapping = true;
                    break;
                }
            }

            if (!isOverlapping)
            {
                // Add the new room to the list
                rooms.Add(newRoom);

                // Carve out the room in the dungeon layout
                for (int x = roomX; x < roomX + roomWidth; x++)
                {
                    for (int y = roomY; y < roomY + roomHeight; y++)
                    {
                        dungeon[x, y] = 0; // Set as a floor tile
                    }
                }
            }
        }

        // Connect the rooms with pathways
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            ConnectRooms(rooms[i], rooms[i + 1], dungeon);
        }

        // Place the generated tiles in the Tilemaps
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (dungeon[x, y] == 0)
                {
                    groundTilemap.SetTile(tilePosition, floorTile);
                }
                else if (dungeon[x, y] == 1)
                {
                    wallTilemap.SetTile(tilePosition, wallTile);
                }
                else if (dungeon[x, y] == 2)
                {
                    pathwayTilemap.SetTile(tilePosition, pathwayTile);
                }
            }
        }
    }

    private void ConnectRooms(Room roomA, Room roomB, int[,] dungeon)
    {
        // Calculate the center of each room
        int roomACenterX = roomA.x + roomA.width / 2;
        int roomACenterY = roomA.y + roomA.height / 2;
        int roomBCenterX = roomB.x + roomB.width / 2;
        int roomBCenterY = roomB.y + roomB.height / 2;

        // Connect the rooms with a straight line (horizontal or vertical)
        if (Random.value < 0.5f)
        {
            // Connect horizontally first, then vertically
            CreateHorizontalPathway(roomACenterX, roomBCenterX, roomACenterY, dungeon);
            CreateVerticalPathway(roomACenterY, roomBCenterY, roomBCenterX, dungeon);
        }
        else
        {
            // Connect vertically first, then horizontally
            CreateVerticalPathway(roomACenterY, roomBCenterY, roomACenterX, dungeon);
            CreateHorizontalPathway(roomACenterX, roomBCenterX, roomBCenterY, dungeon);
        }
    }


    private void CreateHorizontalPathway(int startX, int endX, int y, int[,] dungeon)
    {
        for (int x = Mathf.Min(startX, endX); x <= Mathf.Max(startX, endX); x++)
        {
            dungeon[x, y] = 2; // Set as a pathway tile
        }
    }

    private void CreateVerticalPathway(int startY, int endY, int x, int[,] dungeon)
    {
        for (int y = Mathf.Min(startY, endY); y <= Mathf.Max(startY, endY); y++)
        {
            dungeon[x, y] = 2; // Set as a pathway tile
        }
    }

    private bool CheckRoomOverlap(Room newRoom, Room existingRoom)
    {
        if (newRoom.x < existingRoom.x + existingRoom.width &&
            newRoom.x + newRoom.width > existingRoom.x &&
            newRoom.y < existingRoom.y + existingRoom.height &&
            newRoom.y + newRoom.height > existingRoom.y)
        {
            return true; // Rooms overlap
        }
        return false; // Rooms don't overlap
    }
}
