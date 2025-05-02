using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PixelToTileConverter : MonoBehaviour
{
    [SerializeField] private List<PixelTile> pixelTiles;
    public static Dictionary<Color, WorldTile> TileLookup;


    private void Awake()
    {
        TileLookup = pixelTiles.ToDictionary(p => p.pixel.Color, p => p.tile);
    }
}


[System.Serializable]
public class PixelTile
{
    public PixelSO pixel;
    public WorldTile tile;
}
