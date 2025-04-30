using UnityEngine;
using UnityEngine.Rendering;

public class GlobalNeighborCheckFucntions : MonoBehaviour
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

    public static bool MooreCheck(int arrayX, int arrayY, WorldGenerator worldGenerator, int mooreNeighborhoodSize, PixelSO pixelToCheckFor, int threshold)
    {
        PixelSO[,] pixels = worldGenerator.RetrievePixels();
        int amount = 0;

        for (int y = -mooreNeighborhoodSize; y <= mooreNeighborhoodSize; y++)
        {
            for (int x = -mooreNeighborhoodSize; x <= mooreNeighborhoodSize; x++)
            {
                if (x == 0 && y == 0) continue;

                if (worldGenerator.IsInBounds(x + arrayX, y + arrayY))
                {
                    if (pixels[x + arrayX, y + arrayY] == pixelToCheckFor)
                    {
                        amount++;
                        if (amount > threshold) return true;
                    }
                }
            }
        }

        return false;
    }
}
