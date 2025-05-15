using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Sunlight Mutator", menuName = "Scriptable Objects/World Mutator/Pixel Data/Sunlight")]
public class SunlightMutator : WorldMutatorSO
{
    [Header("Thresholds")]
    [SerializeField, Range(0.0f, 1.0f)] private float veryWarm = 0.8f;
    [SerializeField, Range(0.0f, 1.0f)] private float warm = 0.6f;
    [SerializeField, Range(0.0f, 1.0f)] private float neutral = 0.4f;
    [SerializeField, Range(0.0f, 1.0f)] private float cold = 0.2f;

    [Header("Settings")]
    [SerializeField] private Perlin2DSettings noiseSettings;
    [SerializeField, Range(0.0f, 1.0f)] private float xModifer = 1f;
    [SerializeField, Range(0.0f, 1.0f)] private float yModifer = 1f;

    public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        base.SetUp(worldGenerator, worldSize);

    }

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

                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);

                float dx = arrayX - centerX;
                float dy = arrayY - centerY;
                float distanceFromCenter = Mathf.Sqrt(dx * dx * xModifer + dy * dy * yModifer);
                float maxDistance = Mathf.Sqrt(centerX * centerX + centerY * centerY);
                float radialFactor = 1f - Mathf.InverseLerp(0f, maxDistance, distanceFromCenter);
                float coreTemperature = noiseValue * radialFactor;

                float finalTemperature = coreTemperature;

                if (finalTemperature >= veryWarm)
                {
                    pixelInstance.SunlightLevel = 2;
                }
                else if (finalTemperature >= warm)
                {
                    pixelInstance.SunlightLevel = 1;
                }
                else if (finalTemperature >= neutral)
                {
                    pixelInstance.SunlightLevel = 0;
                }
                else if (finalTemperature >= cold)
                {
                    pixelInstance.SunlightLevel = -1;
                }
                else
                {
                    pixelInstance.SunlightLevel = -2;
                }

                pixels[arrayX, arrayY] = pixelInstance;
            }
        }

        yield return null;
    }
}
