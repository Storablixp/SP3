using UnityEngine;
using System.Collections;
using Unity.Burst.Intrinsics;

[CreateAssetMenu(fileName = "Wetness Mutator", menuName = "Scriptable Objects/World Mutator/Pixel Data/Wetness")]
public class WetnessMutator : WorldMutatorSO
{
    [Header("Thresholds")]
    [SerializeField, Range(0.0f, 1.0f)] private float veryWet = 0.8f;
    [SerializeField, Range(0.0f, 1.0f)] private float wet = 0.6f;
    [SerializeField, Range(0.0f, 1.0f)] private float neutral = 0.4f;
    [SerializeField, Range(0.0f, 1.0f)] private float dry = 0.2f;

    [Header("Settings")]
    public Perlin2DSettings noiseSettings;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                if (noiseValue >= veryWet)
                {
                    pixelInstance.Wetness = 2;
                }
                else if (noiseValue >= wet)
                {
                    pixelInstance.Wetness = 1;
                }
                else if (noiseValue >= neutral)
                {
                    pixelInstance.Wetness = 0;
                }
                else if (noiseValue >= dry)
                {
                    pixelInstance.Wetness = -1;
                }
                else
                {
                    pixelInstance.Wetness = -2;
                }

                pixels[arrayX, arrayY] = pixelInstance;
            }
        }

        yield return null;
    }
}
