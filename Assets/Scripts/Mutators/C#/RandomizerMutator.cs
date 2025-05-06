using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Randomizer Mutator", menuName = "Scriptable Objects/World Mutator/Randomizer")]
public class RandomizerMutator : WorldMutatorSO
{
    public Perlin2DSettings noiseSettings;

    [Header("Settings")]
    [Range(0.0f, 1.0f)] public float PercentageForA;

    [Header("Pixels")]
    public PixelSO PixelA;
    public PixelSO PixelB;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);

                if (noiseValue <= PercentageForA)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, PixelA);
                }
                else
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, PixelB);
                }
            }
        }

        yield return null;
    }
}
