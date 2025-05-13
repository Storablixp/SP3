using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Depth Mutator", menuName = "Scriptable Objects/World Mutator/Biome Data/Depth")]
public class DepthMutator : WorldMutatorSO
{
    [Header("Thresholds")]
    [SerializeField, Range(0.0f, 1.0f)] private float airLayer = 0.8f;
    [SerializeField, Range(0.0f, 1.0f)] private float surfaceLayer = 0.6f;
    [SerializeField, Range(0.0f, 1.0f)] private float caveLayer = 0.4f;
    [SerializeField, Range(0.0f, 1.0f)] private float deepLayer = 0.2f;

    [Header("Settings")]
    [SerializeField] private Perlin2DSettings noiseSettings;
    [SerializeField, Range(0, 100)] private int heightVariationStrength = 50;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);
                float yMod = arrayY + (noiseValue - 0.5f) * 2f * heightVariationStrength;

                if (yMod > worldSize.y * airLayer)
                {
                    pixelInstance.Depth = 2;
                }
                else if (yMod > worldSize.y * surfaceLayer)
                {
                    pixelInstance.Depth = 1;
                }
                else if (yMod > worldSize.y * caveLayer)
                {
                    pixelInstance.Depth = 0;
                }
                else if (yMod > worldSize.y * deepLayer)
                {
                    pixelInstance.Depth = -1;
                }
                else
                {
                    pixelInstance.Depth = -2;
                }

                pixels[arrayX, arrayY] = pixelInstance;
            }
        }

        yield return null;
    }
}
