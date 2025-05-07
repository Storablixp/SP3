using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Cheese Cave Mutator", menuName = "Scriptable Objects/World Mutator/Cave System/Cheese Cave")]
public class CheeseCaveMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO hollowPixel;
    [SerializeField] private PixelSO stonePixel;

    [Header("Settings")]
    [SerializeField] private Perlin2DSettings noiseScettings;
    [SerializeField, Range(0.0f, 1.0f)] private float holeThreshold = 0.8f;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelSO[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                if (pixels[arrayX, arrayY] == stonePixel)
                {
                    float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseScettings);

                    if (noiseValue > holeThreshold)
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, hollowPixel);
                    }
                }
            }
        }

        yield return null;
    }
}
