using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Temperature Mutator", menuName = "Scriptable Objects/World Mutator/Temperature")]
public class TemperatureMutator : WorldMutatorSO
{
    [Header("Settings")]
    public Perlin2DSettings noiseSettings;
    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);
                float depthFactor = (float)arrayY / (worldSize.y - 1);
                float finalTemperature = noiseValue * depthFactor;
                worldGenerator.SetTemperature(arrayX, arrayY, finalTemperature);
            }
        }

        yield return null;
    }
}
