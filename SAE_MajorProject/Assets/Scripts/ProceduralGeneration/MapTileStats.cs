using UnityEngine;

public class MapTileStats : MonoBehaviour
{
    public float wetValue;     // how wet this tile is 
    public float heatValue;    // how hot this tile is

    public TileType CurrentTileType = TileType.Water;   // what type of tile this is

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

    public void CalculateType()
    {
        if(wetValue >= 0.5f)
        {
            if(heatValue > 0.52f)
            {
                CurrentTileType = TileType.RainForest;
            }
            
            else if(heatValue < 0.52f && heatValue >= 0.47f)
            {
                CurrentTileType = TileType.Swamp;
            }
            else
            {
                CurrentTileType = TileType.Forest;
            }
        }
        else
        {
            if(heatValue > 0.52f)
            {
                CurrentTileType = TileType.Desert;
            }
            else if(heatValue <= 0.52f && heatValue >= 0.47f)
            {
                CurrentTileType = TileType.Plains;
            }
            else
            {
                CurrentTileType = TileType.Snowlands;
            }
        }
    }

    public void SetType()
    {
        Color Colour = Color.white;
        switch(CurrentTileType)
        {
            case TileType.Plains:
            {
                Colour = Color.green;
                break;
            }
            case TileType.Desert:
            {
                Colour = Color.yellow;
                break;
            }
            case TileType.RainForest:
            {
                Colour = Color.magenta;
                break;
            }
            case TileType.Snowlands:
            {
                Colour = Color.cyan;
                break;
            }
            case TileType.Forest:
            {
                Colour = Color.grey;
                break;
            }
            case TileType.Swamp:
            {
                Colour = Color.black;
                break;
            }
            case TileType.Water:
            {
                Colour = Color.blue;
                break;
            }
        }

        this.gameObject.GetComponent<SpriteRenderer>().color = Colour;
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
