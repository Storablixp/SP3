using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Terrain Mutator", menuName = "Scriptable Objects/World Mutator/Terrain")]
public class TerrainMutator : WorldMutatorSO
{
    [Header("Settings")]
    public Perlin2DSettings noiseSettings;
    public float heightVariationStrength = 30f;
    [Range(0.001f, 1f)] public float airThreshold = 0.9f;
    [Range(0.001f, 1f)] public float dirtThreshold = 0.75f;

    [Header("Pixels")]
    public PixelSO airPixel;
    public PixelSO dirtPixel;
    public PixelSO stonePixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);
                float yMod = arrayY + (noiseValue - 0.5f) * 2f * heightVariationStrength;

                PixelSO pixelToAdd;

                if (yMod > worldSize.y * airThreshold)
                    pixelToAdd = airPixel;
                else if (yMod > worldSize.y * dirtThreshold)
                    pixelToAdd = dirtPixel;
                else
                    pixelToAdd = stonePixel;

                worldGenerator.ChangePixel(arrayX, arrayY, pixelToAdd);
            }
        }

        yield return null;
    }
}
