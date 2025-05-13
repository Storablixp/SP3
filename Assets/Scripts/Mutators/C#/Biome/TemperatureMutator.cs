using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Temperature Mutator", menuName = "Scriptable Objects/World Mutator/Pixel Data/Temperature")]
public class TemperatureMutator : WorldMutatorSO
{
    [SerializeField, Range(-4f, 2f)] private float x;
    [SerializeField, Range(0.1f, 2f)] private float y;
    [SerializeField, Range(0.1f, 4f)] private float multiplier;

    [Header("Settings")]
    public Perlin2DSettings noiseSettings;
    public Perlin1DSettings perlin1D;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        float centerX = worldSize.x / x;
        float centerY = worldSize.y / y;

        float centerX2 = worldSize.x / 2;
        float centerY2 = worldSize.y / 2;

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);

                float dx = arrayX - centerX;
                float dy = arrayY - centerY;
                float distanceFromCenter = Mathf.Sqrt(dx * dx + dy * dy);
                float maxDistance = Mathf.Sqrt(centerX * centerX + centerY * centerY);
                float radialFactor = multiplier - Mathf.InverseLerp(0f, maxDistance, distanceFromCenter);
                float coreTemperature = noiseValue * radialFactor;
               
                float finalTemperature = coreTemperature;

                if (finalTemperature >= 0.8f)
                {
                    pixelInstance.Temperature = 2;
                }
                else if (finalTemperature >= 0.6f)
                {
                    pixelInstance.Temperature = 1;
                }
                else if (finalTemperature >= 0.4f)
                {
                    pixelInstance.Temperature = 0;
                }
                else if (finalTemperature >= 0.2f)
                {
                    pixelInstance.Temperature = -1;
                }
                else
                {
                    pixelInstance.Temperature = -2;
                }


                pixels[arrayX, arrayY] = pixelInstance;
            }
        }

        yield return null;
    }
}
