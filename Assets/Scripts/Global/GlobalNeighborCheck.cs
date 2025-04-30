using UnityEngine;
using UnityEngine.Rendering;

public class GlobalNeighborCheck : MonoBehaviour
{
    //public static bool CheckNeighbor(WorldGenerator worldGenerator, WorldChunk chunk, WorldTile typeToCheckFor, Vector2Int direction)
    //{
    //    bool found = false;

    //    if (worldGenerator.IsInBounds(direction.x, direction.y))
    //    {
    //        if (chunk.Tiles[direction.x, direction.y].WorldTile == typeToCheckFor)
    //        {
    //            found = true;
    //        }
    //    }

    //    return found;
    //}

    //public static bool CheckNeighbors(int arrayX, int arrayY, int size, WorldGenerator worldGenerator, WorldChunk chunk, WorldTile typeToCheckFor, int threshold)
    //{
    //    int amount = 0;

    //    for (int y = -size; y <= size; y++)
    //    {
    //        for (int x = -size; x <= size; x++)
    //        {
    //            if (x == 0 && y == 0) continue;

    //            if (worldGenerator.IsInBounds(x + arrayX, y + arrayY))
    //            {
    //                if (chunk.Tiles[x + arrayX, y + arrayY].WorldTile == typeToCheckFor)
    //                {
    //                    amount++;
    //                    if (amount >= threshold) return true;
    //                }
    //            }
    //        }
    //    }

    //    return false;
    //}
}
