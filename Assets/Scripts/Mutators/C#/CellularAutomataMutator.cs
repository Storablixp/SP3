using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Cellular Automata Mutator", menuName = "Scriptable Objects/World Mutator/Cellular Automata")]
public class CellularAutomataMutator : WorldMutatorSO
{
    [Header("Pixels")]
    public PixelSO PixelToCheckFor;

    [Header("Settings")]
    public int ReplacementThreshold = 4;
    public int MooreNeighborhoodSize = 1;
    public uint Iterations = 5;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] currentPixels = worldGenerator.RetrievePixels();
        PixelInstance[,] updatedPixels = new PixelInstance[worldSize.x, worldSize.y];

        for (int i = 0; i < Iterations; i++)
        {
            // First pass: calculate new states
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
                {
                    PixelSO pixel = currentPixels[arrayX, arrayY].Pixel;
                    if (pixel == null || pixel == PixelToCheckFor) continue;

                    if (GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, MooreNeighborhoodSize, PixelToCheckFor, ReplacementThreshold))
                    {
                        updatedPixels[arrayX, arrayY].Pixel = PixelToCheckFor;
                    }
                    else
                    {
                        updatedPixels[arrayX, arrayY].Pixel = pixel;
                    }
                }
            }

            // Second pass: apply changes
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
                {
                    PixelSO pixel = currentPixels[arrayX, arrayY].Pixel;
                    if (pixel == null) continue;

                    PixelSO newPixel = updatedPixels[arrayX, arrayY].Pixel;
                    if (newPixel == null) continue;

                    pixel = newPixel;

                    worldGenerator.ChangePixel(arrayX, arrayY, newPixel);
                }
            }
        }

        yield return null;
    }
}
