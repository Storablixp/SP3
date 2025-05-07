using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Temperature Mutator", menuName = "Scriptable Objects/World Mutator/Pixel Data/Temperature")]
public class TemperatureMutator : WorldMutatorSO
{
    public float[,] Temperatures;

    [Header("Settings")]
    public Perlin2DSettings noiseSettings;

    public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        base.SetUp(worldGenerator, worldSize);
        Temperatures = new float[worldSize.x, worldSize.y];
    }

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D((float)arrayX, (float)arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);
                //float depthFactor = (float)arrayY / (worldSize.y - 1);
                //float finalTemperature = noiseValue += depthFactor;
                //finalTemperature = Mathf.Clamp01(finalTemperature);
                Temperatures[arrayX, arrayY] = noiseValue;
            }
        }

        yield return null;
    }
}
