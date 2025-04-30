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
        PixelSO[,] updatedPixelData = new PixelSO[worldSize.x, worldSize.y];
        PixelSO[,] originalPixels = worldGenerator.RetrievePixels();

        for (int i = 0; i < Iterations; i++)
        {
            if (visuals && i != 0) worldGenerator.ResetCounterValues();

            // First pass: calculate new states
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
                {
                    PixelSO pixelInstance = originalPixels[arrayX, arrayY];
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
                                PixelSO neighbor = originalPixels[nx, ny];
                                if (neighbor != null && neighbor == PixelToCount)
                                {
                                    desiredNeighbors++;
                                }
                            }
                        }
                    }

                    if (desiredNeighbors > ReplacementThreshold)
                    {
                        updatedPixelData[arrayX, arrayY] = PixelToCount;
                    }
                    else updatedPixelData[arrayX, arrayY] = PixelToReplace;
                }
            }

            // Second pass: apply changes
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
                {
                    PixelSO pixelInstance = originalPixels[arrayX, arrayY];
                    if (pixelInstance == null) continue;
                    if (pixelInstance != PixelToReplace && pixelInstance != PixelToCount) continue;

                    PixelSO newPixel = updatedPixelData[arrayX, arrayY];
                    if (newPixel == null) continue;

                    pixelInstance = newPixel;

                    worldGenerator.AddPixel(arrayX, arrayY, newPixel);
                }
            }
        }

        yield return null;
    }
}
