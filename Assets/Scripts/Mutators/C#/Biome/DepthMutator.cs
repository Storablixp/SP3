using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Depth Mutator", menuName = "Scriptable Objects/World Mutator/Pixel Data/Depth")]
public class DepthMutator : WorldMutatorSO
{
    [Header("Settings")]
    public Perlin2DSettings noiseSettings;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);
                float depthFactor = (float)arrayY / (worldSize.y - 1);
                float finalDepth = noiseValue + depthFactor;
                finalDepth = Mathf.Clamp01(finalDepth);
                pixels[arrayX, arrayY].Depth = finalDepth;
            }
        }

        yield return null;
    }
}
