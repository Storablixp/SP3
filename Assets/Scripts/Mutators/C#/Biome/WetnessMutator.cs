using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Wetness Mutator", menuName = "Scriptable Objects/World Mutator/Biome Data/Wetness")]
public class WetnessMutator : WorldMutatorSO
{
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
                pixels[arrayX, arrayY].Wetness = noiseValue;
            }
        }

        yield return null;
    }
}
