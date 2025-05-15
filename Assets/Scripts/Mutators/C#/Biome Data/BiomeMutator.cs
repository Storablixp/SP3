using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Biome Mutator", menuName = "Scriptable Objects/World Mutator/Biome Data/Biome")]
public class BiomeMutator : WorldMutatorSO
{
    [Header("Thresholds")]
    [SerializeField, Range(0.0f, 1.0f)] private float veryWarm = 0.8f;
    [SerializeField, Range(0.0f, 1.0f)] private float warm = 0.6f;
    [SerializeField, Range(0.0f, 1.0f)] private float neutral = 0.4f;
    [SerializeField, Range(0.0f, 1.0f)] private float cold = 0.2f;

    [Header("Settings")]
    [SerializeField] private Perlin1DSettings noiseSettings;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        float centerX = worldSize.x / 2f;
        float centerY = worldSize.y / 1.25f;

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise1D(arrayX, WorldGenerator.XOffset, noiseSettings);

                if (noiseValue >= veryWarm)
                {
                    pixelInstance.Biome = 2;
                }
                else if (noiseValue >= warm)
                {
                    pixelInstance.Biome = 1;
                }
                else if (noiseValue >= neutral)
                {
                    pixelInstance.Biome = 0;
                }
                else if (noiseValue >= cold)
                {
                    pixelInstance.Biome = -1;
                }
                else
                {
                    pixelInstance.Biome = -2;
                }

                pixels[arrayX, arrayY] = pixelInstance;
            }
        }

        yield return null;
    }
}
