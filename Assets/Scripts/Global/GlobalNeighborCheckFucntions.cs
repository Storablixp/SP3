using UnityEngine;
using UnityEngine.Rendering;

public class GlobalNeighborCheckFucntions : MonoBehaviour
{
    public static bool SimpleCheck(int arrayX, int arrayY, Vector2Int direction, WorldGenerator worldGenerator, PixelSO pixelToCheckFor)
    {
        PixelSO[,] pixels = worldGenerator.RetrievePixels();

        if (worldGenerator.IsInBounds(arrayX + direction.x, arrayY + direction.y))
        {
            if (pixels[arrayX + direction.x, arrayY + direction.y] == pixelToCheckFor)
            {
               return true;
            }
        }
        return false;
    }

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
