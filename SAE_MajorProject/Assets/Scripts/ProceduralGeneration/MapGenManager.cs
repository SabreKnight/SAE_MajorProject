using UnityEngine;

public class MapGenManager : MonoBehaviour
{
    [SerializeField] private int mapSizeWidth = 255;    // how wide the map is horizontally
    [SerializeField] private int mapSizeHeight = 255;   // how wide the map is vertically

    [SerializeField] private Vector2 ArrayOffset = new Vector2(-128f, -128f);   // offsets the map tiles form the world position

    [SerializeField] private Grid grid; // a grid component reference

    private MapTileStats[,] mapTileArray;   // array of map tile stat components


    private void InitialMapGeneration() // creates the 2D array of map tile stats
    {
        for(int x = 0; x < mapSizeWidth; x++)   // loops all horizonatal items
        {
            for(int y = 0; y < mapSizeHeight; y++)  // loops all vertical items
            {
                MapTileStats currentTile = CreateArray(x,y);    // creates a map tile stat object for that location
            }
        }
    }

    private void ApplyStats(MapTileStats currentTile)   // applies the stat values to the tile 
    {

    }

    private MapTileStats CreateArray(int x, int y)  // creates a tile spot for that location
    {
        MapTileStats stats = null;  // creates a temp reference
        
        if(mapTileArray[x,y] == null)   // checks if there is already a tile object in that location
        {   // if there is no object
            // creates a new game object with a map tile and sprite renderer component
            GameObject mapTile = new GameObject("mapTile", typeof(MapTileStats), typeof(SpriteRenderer));
            stats = mapTile.GetComponent<MapTileStats>();   // gets the reference for that tile
            
        }

        else    // if there is already an object in that position
        {
            stats = mapTileArray[x,y];  // just gets the reference for that object
        }

        stats.SetPosition(x, y, ArrayOffset);    // sets the position of that object within the stats class

        return stats;   // returns the stat object reference.
        
    }


}
