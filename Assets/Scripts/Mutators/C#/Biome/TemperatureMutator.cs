using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Temperature Mutator", menuName = "Scriptable Objects/World Mutator/Pixel Data/Temperature")]
public class TemperatureMutator : WorldMutatorSO
{
    [Header("Settings")]
    public Perlin2DSettings noiseSettings;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        float centerX = worldSize.x / 2f;
        float centerY = worldSize.y / 2f;

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);
                float dx = arrayX - centerX;
                float dy = arrayY - centerY;
                float distanceFromCenter = Mathf.Sqrt(dx * dx + dy * dy);
                float maxDistance = Mathf.Sqrt(centerX * centerX + centerY * centerY);
                float radialFactor = 1f - Mathf.InverseLerp(0f, maxDistance, distanceFromCenter);
                float coreTemperature = noiseValue * radialFactor;
                pixels[arrayX, arrayY].Temperature = Mathf.Clamp01(coreTemperature);
            }
        }

        yield return null;
    }
}
