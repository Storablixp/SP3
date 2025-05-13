using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "Sand And Snow Mutator", menuName = "Scriptable Objects/World Mutator/Sand And Snow")]
public class SandAndSnowMutator : WorldMutatorSO
{
    [Header("Settings")]
    [Range(0.0f, 1.0f), SerializeField] private float SandThreshold;
    [Range(0.0f, 1.0f), SerializeField] private float ClayThreshold;

    [Header("Pixels")]
    [SerializeField] private PixelSO DirtPixel;
    [SerializeField] private PixelSO SandPixel;
    [SerializeField] private PixelSO SnowPixel;
    [SerializeField] private PixelSO ClayPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];

                if (pixel.Pixel == DirtPixel)
                {
                    if (pixel.SunlightLevel > SandThreshold)
                    {
                        if (pixel.Wetness > ClayThreshold)
                        {
                            worldGenerator.ChangePixel(arrayX, arrayY, ClayPixel);
                        }
                        else worldGenerator.ChangePixel(arrayX, arrayY, SandPixel);
                    }
                    else
                    {
                        if (pixel.SunlightLevel < 0.2f)
                        {
                            worldGenerator.ChangePixel(arrayX, arrayY, SnowPixel);
                        }
                    }
                }
            }
        }

        yield return null;
    }
}
