using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Cellular Automata Mutator", menuName = "Scriptable Objects/World Mutator/Cellular Automata")]
public class CellularAutomataMutator : WorldMutatorSO
{
    [Header("Pixels")]
    public PixelSO PixelToCheckFor;
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
                    if (pixelInstance != PixelToReplace && pixelInstance != PixelToCheckFor) continue;


                    if (GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, MooreNeighborhoodSize, PixelToCheckFor, ReplacementThreshold))
                    {
                        updatedPixels[arrayX, arrayY] = PixelToCheckFor;
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
                    if (pixelInstance != PixelToReplace && pixelInstance != PixelToCheckFor) continue;

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
