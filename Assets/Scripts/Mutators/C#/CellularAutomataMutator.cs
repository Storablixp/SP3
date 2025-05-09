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

    //public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    //{
    //    base.SetUp(worldGenerator, worldSize);
    //}

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
                    PixelSO pixel = currentPixels[arrayX, arrayY].pixel;
                    if (pixel == null) continue;
                    if (pixel != PixelToReplace && pixel != PixelToCheckFor) continue;

                    if (GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, MooreNeighborhoodSize, PixelToCheckFor, ReplacementThreshold))
                    {
                        updatedPixels[arrayX, arrayY].pixel = PixelToCheckFor;
                    }
                    else updatedPixels[arrayX, arrayY].pixel = PixelToReplace;
                }
            }

            // Second pass: apply changes
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
                {
                    PixelSO pixel = currentPixels[arrayX, arrayY].pixel;
                    if (pixel == null) continue;
                    if (pixel != PixelToReplace && pixel != PixelToCheckFor) continue;

                    PixelSO newPixel = updatedPixels[arrayX, arrayY].pixel;
                    if (newPixel == null) continue;

                    pixel = newPixel;

                    worldGenerator.ChangePixel(arrayX, arrayY, newPixel);
                }
            }
        }

        yield return null;
    }
}
