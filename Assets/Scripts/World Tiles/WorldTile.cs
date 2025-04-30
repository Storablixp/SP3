using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New WorldTile", menuName = "Scriptable Objects/World Tile")]
public class WorldTile : TileBase
{
    public Sprite CurrentSprite;
    public Color Color = Color.white;
    public Tile.ColliderType ColliderType;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = CurrentSprite;
        tileData.color = Color;
        tileData.colliderType = ColliderType;
    }
}