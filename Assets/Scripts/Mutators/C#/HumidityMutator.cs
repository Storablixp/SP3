using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Humidity Mutator", menuName = "Scriptable Objects/World Mutator/Pixel Data/Humidity")]
public class HumidityMutator : WorldMutatorSO
{
    [Header("Settings")]
    public Perlin2DSettings noiseSettings;

    public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        base.SetUp(worldGenerator, worldSize);
    }

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);
                pixels[arrayX, arrayY].Humidity = noiseValue;
            }
        }

        yield return null;
    }
}
