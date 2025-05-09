using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "Sand Placer Mutator", menuName = "Scriptable Objects/World Mutator/Sand Placer")]
public class SandPlacerMutator : WorldMutatorSO
{
    private List<Vector2Int> sandPixels = new();
    private PixelInstance[,] pixels;

    [Header("Settings")]
    [Range(0.0f, 1.0f), SerializeField] private float SandThreshold;
    [Range(0.0f, 1.0f), SerializeField] private float ClayThreshold;
    [Range(0.0f, 1.0f), SerializeField] private float WaterThreshold;

    [Header("Pixels")]
    [SerializeField] private PixelSO DirtPixel;
    [SerializeField] private PixelSO StonePixel;
    [SerializeField] private PixelSO SandPixel;
    [SerializeField] private PixelSO ClayPixel;
    [SerializeField] private PixelSO WaterPixel;

    public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        base.SetUp(worldGenerator, worldSize);
        sandPixels.Clear();
    }

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];

                if (pixel.Pixel == DirtPixel || pixel.Pixel == StonePixel)
                {
                    if (pixel.Temperature > SandThreshold)
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, SandPixel);
                        sandPixels.Add(new Vector2Int(arrayX, arrayY));
                    }
                }
            }
        }

        for (int i = 0; i < sandPixels.Count; i++)
        {
            if (pixels[sandPixels[i].x, sandPixels[i].y].Humidity > ClayThreshold)
            {
                worldGenerator.ChangePixel(sandPixels[i].x, sandPixels[i].y, ClayPixel);
            }
            else if (pixels[sandPixels[i].x, sandPixels[i].y].Humidity < WaterThreshold)
            {
                worldGenerator.ChangePixel(sandPixels[i].x, sandPixels[i].y, WaterPixel);
            }
        }

        yield return null;
    }
}
