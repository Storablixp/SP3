using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Spaghetti Cave Mutator", menuName = "Scriptable Objects/World Mutator/Cave System/Spaghetti Cave")]
public class SpaghettiCaveMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO hollowPixel;
    [SerializeField] private PixelSO airPixel;

    [Header("Settings")]
    [SerializeField, Range(0.4f, 0.5f)] private float thickness;
    [SerializeField] private Perlin2DSettings noiseScettings;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];
                if (pixelInstance.Pixel != airPixel)
                {
                    if (pixelInstance.Temperature == 0)
                    {
                        float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseScettings);

                        if (noiseValue > (0 + thickness) && noiseValue < (1 - thickness))
                        {
                            worldGenerator.ChangePixel(arrayX, arrayY, hollowPixel);
                        }
                    }
                    else if (pixelInstance.Temperature == 1 && pixelInstance.Depth == 0)
                    {
                        float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseScettings);

                        if (noiseValue > (0 + thickness) && noiseValue < (1 - thickness))
                        {
                            worldGenerator.ChangePixel(arrayX, arrayY, hollowPixel);
                        }
                    }
                    else if (pixelInstance.Depth <= -1)
                    {
                        float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseScettings);

                        if (noiseValue > (0 + thickness) && noiseValue < (1 - thickness))
                        {
                            worldGenerator.ChangePixel(arrayX, arrayY, hollowPixel);
                        }
                    }
                }
            }
        }

        yield return null;
    }
}
