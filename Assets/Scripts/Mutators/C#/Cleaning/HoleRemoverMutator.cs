using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Hole Remover", menuName = "Scriptable Objects/World Mutator/Cleaning/Hole Remover")]

public class HoleRemoverMutator : WorldMutatorSO
{
    [Header("Other Pixels")]
    [SerializeField] private PixelSO borderPixel;
    [SerializeField] private PixelSO hollowPixel;

    [Header("Fill Pixels")]
    [SerializeField] private PixelSO dirtPixel;
    [SerializeField] private PixelSO stonePixel;
    [SerializeField] private PixelSO deepStonePixel;

    [Header("Settings")]
    [SerializeField] private uint iterations;
    [SerializeField, Range(0, 12)] private uint minimumNeighborHoles;


    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int iteration  = 0; iteration < iterations; iteration++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
                {
                    PixelInstance pixel = pixels[arrayX, arrayY];
                    if (pixel.Pixel == hollowPixel)
                    {
                        List<Vector2Int> pixelsWithinCircle = new();
                        for (int y = -2; y <= 2; y++)
                        {
                            for (int x = -2; x <= 2; x++)
                            {
                                if (y == -2 || y == 2)
                                {
                                    if (x == -2 || x == 2) continue;
                                }

                                if (!worldGenerator.IsInBounds(arrayX + x, arrayY + y)) continue;

                                if (pixels[arrayX + x, arrayY + y].Pixel == hollowPixel)
                                    pixelsWithinCircle.Add(new Vector2Int(arrayX + x, arrayY + y));
                            }
                        }

                        uint hollowPixelsWithinCircle = 0;
                        for (int i = 0; i < pixelsWithinCircle.Count; i++)
                        {
                            if (pixels[pixelsWithinCircle[i].x, pixelsWithinCircle[i].y].Pixel == hollowPixel)
                            {
                                hollowPixelsWithinCircle++;
                            }
                        }

                        if (hollowPixelsWithinCircle < minimumNeighborHoles)
                        {
                            for (int i = 0; i < pixelsWithinCircle.Count; i++)
                            {
                                PixelInstance currentPixel = pixels[pixelsWithinCircle[i].x, pixelsWithinCircle[i].y];

                                if (currentPixel.Pixel != borderPixel)
                                {
                                    PixelSO pixelToReplaceWith;
                                    if (currentPixel.Depth == 1) pixelToReplaceWith = dirtPixel;
                                    else if (currentPixel.Depth == 0) pixelToReplaceWith = stonePixel;
                                    else if (currentPixel.Depth <= -1) pixelToReplaceWith = deepStonePixel;
                                    else continue;

                                    worldGenerator.ChangePixel(pixelsWithinCircle[i].x, pixelsWithinCircle[i].y, pixelToReplaceWith);
                                }
                            }
                        }
                    }
                }
            }
        }

        yield return null;
    }
}
