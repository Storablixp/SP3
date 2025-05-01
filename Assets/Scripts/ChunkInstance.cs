using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ChunkInstance : MonoBehaviour
{
    public ChunkInstance[,] NeighborChunks = new ChunkInstance[3, 3];
    public Tilemap Tilemap;
    public Vector2Int CenterPos;
    public BoundsInt Bounds;
}
