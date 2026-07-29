using UnityEngine;

public class MapTileStats : MonoBehaviour
{
    private float wetValue;     // how wet this tile is 
    private float heatValue;    // how hot this tile is

    private TileType CurrentTileType;   // what type of tile this is

    private int locationHor;    // the location of the tile horizontally
    private int locationVert;   // the location of the tile vertically

    public void SetPosition(int x, int y, Vector2 ArrayOffset)  // sets the position of the tile object
    {
        //sets the horizontal and vertical values of the tile
        locationHor = x;
        locationVert = y;

        // changes the game object position appropriately based on the tile location and tile offset
        this.gameObject.transform.position = new Vector2((float) x, (float) y) + ArrayOffset;
    }
    
}

public enum TileType   // enumerated type labeling the tile types
{
    Plains = 0,
    Desert = 1,
    RainForest = 2,
    Snowlands = 3,
    Forest = 4,
    Swamp = 5,
    Water = 6
}
