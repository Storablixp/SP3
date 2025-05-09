using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Sand Placer Mutator", menuName = "Scriptable Objects/World Mutator/Sand Placer")]
public class SandPlacerMutator : WorldMutatorSO
{
    [Header("Settings")]
    [SerializeField] private Perlin2DSettings noiseSettings;
    [Range(0.0f, 1.0f), SerializeField] private float Threshold;

    [Header("Pixels")]
    public PixelSO DirtPixel;
    public PixelSO StonePixel;
    public PixelSO SandPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                if (pixels[arrayX, arrayY].Pixel == DirtPixel || pixels[arrayX, arrayY].Pixel == StonePixel)
                {
                    float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);

                    if (noiseValue > Threshold)
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, SandPixel);
                    }
                }
            }
        }

        yield return null;
    }
}
