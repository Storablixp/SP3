using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System;

[CreateAssetMenu(fileName = "Hole Remover", menuName = "Scriptable Objects/World Mutator/Cleaning/Hole Remover")]

public class HoleRemoverMutator : WorldMutatorSO
{
    [Header("Other Pixels")]
    [SerializeField] private PixelSO borderPixel;
    [SerializeField] private PixelSO hollowPixel;

    [Header("Fill Pixels")]
    [SerializeField] private PixelSO dirtPixel;
    [SerializeField] private PixelSO snowPixel;
    [SerializeField] private PixelSO sandPixel;
    [SerializeField] private PixelSO stonePixel;
    [SerializeField] private PixelSO deepStonePixel;
    [SerializeField] private PixelSO waterPixel;

    [Header("Settings")]
    [SerializeField] private uint iterations;
    [SerializeField, Range(0, 12)] private uint minimumNeighborHoles;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int iteration = 0; iteration < iterations; iteration++)
        {
            worldGenerator.ResetCounterValues();

            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
                {
                    PixelInstance pixel = pixels[arrayX, arrayY];
                    if (pixel.Pixel != hollowPixel) continue;

                    List<Vector2Int> pixelsWithinCircle = new();

                    foreach (var circlePixel in GetPixelsInCircle(arrayX, arrayY))
                    {
                        if (!worldGenerator.IsInBounds(circlePixel.x, circlePixel.y)) continue;

                        if (pixels[circlePixel.x, circlePixel.y].Pixel == hollowPixel)
                        {
                            pixelsWithinCircle.Add(new Vector2Int(circlePixel.x, circlePixel.y));
                        }
                    }


                    uint hollowPixelsWithinCircle = 0;
                    foreach (var pos in pixelsWithinCircle)
                    {
                        if (pixels[pos.x, pos.y].Pixel == hollowPixel)
                        {
                            hollowPixelsWithinCircle++;
                        }
                    }

                    if (hollowPixelsWithinCircle < minimumNeighborHoles)
                    {
                        foreach (var pos in pixelsWithinCircle)
                        {
                            PixelInstance currentPixel = pixels[pos.x, pos.y];

                            if (currentPixel.Pixel != hollowPixel) continue;

                            if (GlobalNeighborCheckFucntions.MooreCheck(pos.x, pos.y, worldGenerator, 1, waterPixel, 0))
                            {
                                worldGenerator.ChangePixel(pos.x, pos.y, waterPixel);
                            }
                            else
                            {
                                PixelSO pixelToReplaceWith;
                                if (currentPixel.Depth == 1)
                                {
                                    if (currentPixel.Temperature < 0)
                                    {
                                        pixelToReplaceWith = snowPixel;
                                    }
                                    else if (currentPixel.Temperature > 0)
                                    {
                                        pixelToReplaceWith = sandPixel;
                                    }
                                    else pixelToReplaceWith = dirtPixel;
                                }
                                else if (currentPixel.Depth == 0) pixelToReplaceWith = stonePixel;
                                else if (currentPixel.Depth <= -1) pixelToReplaceWith = deepStonePixel;
                                else continue;

                                worldGenerator.ChangePixel(pos.x, pos.y, pixelToReplaceWith);
                            }
                        }
                    }

                    if (worldGenerator.UpdateProgressbar(false))
                    {
                        yield return null;
                    }
                }
            }
        }

        yield return null;
    }

    private Vector2Int[] GetPixelsInCircle(int arrayX, int arrayY)
    {
        List<Vector2Int> pixelCoord = new();
        for (int y = -2; y <= 2; y++)
        {
            for (int x = -2; x <= 2; x++)
            {
                if ((y == -2 || y == 2) && (x == -2 || x == 2)) continue;
                pixelCoord.Add(new Vector2Int(arrayX + x, arrayY + y));
            }
        }
        return pixelCoord.ToArray();
    }
}
