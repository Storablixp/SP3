using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Depth Mutator", menuName = "Scriptable Objects/World Mutator/Biome Data/Depth")]
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

                PixelInstance pixelInstance = pixels[arrayX, arrayY];
                pixelInstance.Depth = finalDepth;

                //if (finalDepth >= 0.8f)
                //{
                //    pixelInstance.Depth = 2;
                //}
                //else if (finalDepth >= 0.6f)
                //{
                //    pixelInstance.Depth = 0;
                //}
                //else if (finalDepth >= 0.4f)
                //{
                //    pixelInstance.Depth = 0;
                //}
                //else if (finalDepth >= 0.2f)
                //{
                //    pixelInstance.Depth = 0;
                //}
                //else
                //{
                //    pixelInstance.Depth = -1;
                //}

                pixels[arrayX, arrayY] = pixelInstance;
            }
        }

        yield return null;
    }
}
