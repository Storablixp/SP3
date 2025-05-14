using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Remove Line", menuName = "Scriptable Objects/World Mutator/Cleaning/Remove Lines")]

public class RemoveLineMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO borderPixel;

    [Header("Settings")]
    [SerializeField, Range(0, 3)] private uint threshold;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];
                if (pixel.Pixel != airPixel)
                {
                    if (!GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 1, pixels[arrayX, arrayY].Pixel, threshold))
                    {
                        if (worldGenerator.IsInBounds(arrayX - 1, arrayY))
                        {
                            PixelSO pixelBeneath = pixels[arrayX - 1, arrayY].Pixel;
                            if (pixelBeneath != borderPixel)
                            {
                                worldGenerator.ChangePixel(arrayX, arrayY, pixelBeneath);
                            }
                        }
                    }
                }
            }
        }

        yield return null;
    }
}
