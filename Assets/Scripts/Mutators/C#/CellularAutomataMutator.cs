using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Cellular Automata Mutator", menuName = "Scriptable Objects/World Mutator/Cellular Automata")]
public class CellularAutomataMutator : WorldMutatorSO
{
    [Header("Pixels")]
    public PixelSO PixelToCount;
    public PixelSO PixelToReplace;

    [Header("Settings")]
    public int ReplacementThreshold = 4;
    public int MooreNeighborhoodSize = 1;
    public int Iterations = 5;

    public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        base.SetUp(worldGenerator, worldSize);
    }

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelSO[,] currentPixels = worldGenerator.RetrievePixels();
        PixelSO[,] updatedPixels = new PixelSO[worldSize.x, worldSize.y];

        for (int i = 0; i < Iterations; i++)
        {
            // First pass: calculate new states
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
                {
                    PixelSO pixelInstance = currentPixels[arrayX, arrayY];
                    if (pixelInstance == null) continue;
                    if (pixelInstance != PixelToReplace && pixelInstance != PixelToCount) continue;


                    int desiredNeighbors = 0;
                    for (int dy = -MooreNeighborhoodSize; dy <= MooreNeighborhoodSize; dy++)
                    {
                        for (int dx = -MooreNeighborhoodSize; dx <= MooreNeighborhoodSize; dx++)
                        {
                            if (dx == 0 && dy == 0) continue;

                            int nx = arrayX + dx;
                            int ny = arrayY + dy;

                            if (worldGenerator.IsInBounds(nx, ny))
                            {
                                PixelSO neighbor = currentPixels[nx, ny];
                                if (neighbor != null && neighbor == PixelToCount)
                                {
                                    desiredNeighbors++;
                                }
                            }
                        }
                    }

                    if (desiredNeighbors > ReplacementThreshold)
                    {
                        updatedPixels[arrayX, arrayY] = PixelToCount;
                    }
                    else updatedPixels[arrayX, arrayY] = PixelToReplace;
                }
            }

            // Second pass: apply changes
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
                {
                    PixelSO pixelInstance = currentPixels[arrayX, arrayY];
                    if (pixelInstance == null) continue;
                    if (pixelInstance != PixelToReplace && pixelInstance != PixelToCount) continue;

                    PixelSO newPixel = updatedPixels[arrayX, arrayY];
                    if (newPixel == null) continue;

                    pixelInstance = newPixel;

                    worldGenerator.ChangePixel(arrayX, arrayY, newPixel);
                }
            }
        }

        yield return null;
    }
}
