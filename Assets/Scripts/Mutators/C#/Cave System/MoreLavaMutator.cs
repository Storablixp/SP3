using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "More Lava Mutator", menuName = "Scriptable Objects/World Mutator/Cave System/More Lava")]
public class MoreLavaMutator : WorldMutatorSO
{
    [Header("Settings")]
    public uint ReplacementThreshold = 4;
    public int MooreNeighborhoodSize = 1;
    public uint Iterations = 5;
    [SerializeField, Range(0, 100)] private int veinThreshold = 1;
    [SerializeField] private Vector2Int veinLength = Vector2Int.one;

    [Header("Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO lavaPixel;
    [SerializeField] private PixelSO volcanicRockPixel;
    [SerializeField] private PixelSO scorchedRockPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        Vector2Int centerRock = FindCenterVolcanicRock(worldSize, pixels);
        int lowestY = CreateVeins(pixels, centerRock);
        AddLavaPoolAtBottom(worldSize, pixels, centerRock, lowestY);
        BiggerVeins(worldSize, pixels);
        CleanUpSpills(worldSize, pixels);
        yield return null;
    }

    private Vector2Int FindCenterVolcanicRock(Vector2Int worldSize, PixelInstance[,] pixels)
    {
        List<Vector2Int> volcanicRocksAtTheTop = new();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                if (pixelInstance.Pixel == volcanicRockPixel)
                {
                    volcanicRocksAtTheTop.Add(new Vector2Int(arrayX, arrayY));
                    break;
                }
            }
        }

        return volcanicRocksAtTheTop[volcanicRocksAtTheTop.Count / 2];
    }
    private int CreateVeins(PixelInstance[,] pixels, Vector2Int centerRock)
    {
        int lowestY = 0;
        int placementCooldown = 0;

        for (int arrayY = centerRock.y; arrayY >= endY; arrayY--)
        {
            PixelInstance pixelInstance = pixels[centerRock.x, arrayY];

            if (pixelInstance.Pixel == volcanicRockPixel || pixelInstance.Pixel == lavaPixel)
            {
                for (int x = -1; x <= 1; x++)
                {
                    worldGenerator.ChangePixel(centerRock.x + x, arrayY, lavaPixel);

                    if (veinThreshold >= Random.Range(0, 101) && placementCooldown <= 0 && arrayY < centerRock.y - 32)
                    {
                        int direction = Random.Range(-1, 2);

                        while (direction == 0)
                        {
                            direction = Random.Range(-1, 2);
                        }

                        int length = Random.Range(veinLength.x, veinLength.y + 1);

                        for (int i = 1; i <= length; i++)
                        {
                            worldGenerator.ChangePixel(centerRock.x + x + (i * direction), arrayY + i, lavaPixel);
                        }

                        placementCooldown = 50;
                    }

                }
                lowestY = arrayY;
                placementCooldown--;
            }
            else break;
        }

        return lowestY;
    }
    private void AddLavaPoolAtBottom(Vector2Int worldSize, PixelInstance[,] pixels, Vector2Int centerRock, int lowestY)
    {
        for (int y = -10; y < 20; y++)
        {
            for (int x = -25; x <= 25; x++)
            {
                worldGenerator.ChangePixel(centerRock.x + x, lowestY + y, lavaPixel);
            }
        }
    }
    private void BiggerVeins(Vector2Int worldSize, PixelInstance[,] pixels)
    {
        PixelInstance[,] currentPixels = pixels;
        PixelInstance[,] updatedPixels = new PixelInstance[worldSize.x, worldSize.y];

        for (int i = 0; i < Iterations; i++)
        {
            // First pass: calculate new states
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
                {
                    PixelSO pixel = currentPixels[arrayX, arrayY].Pixel;
                    if (pixel == null || pixel == lavaPixel) continue;

                    if (GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, MooreNeighborhoodSize, lavaPixel, ReplacementThreshold))
                    {
                        updatedPixels[arrayX, arrayY].Pixel = lavaPixel;
                    }
                    else
                    {
                        updatedPixels[arrayX, arrayY].Pixel = pixel;
                    }
                }
            }

            // Second pass: apply changes
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
                {
                    PixelSO pixel = currentPixels[arrayX, arrayY].Pixel;
                    if (pixel == null || pixel == lavaPixel) continue;

                    PixelSO newPixel = updatedPixels[arrayX, arrayY].Pixel;
                    if (newPixel == null) continue;

                    pixel = newPixel;

                    worldGenerator.ChangePixel(arrayX, arrayY, newPixel);
                }
            }
        }
    }

    private void CleanUpSpills(Vector2Int worldSize, PixelInstance[,] pixels)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = endY; arrayY < startY; arrayY++)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                if (pixelInstance.Pixel == lavaPixel)
                {
                    if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, airPixel))
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, airPixel);
                    }
                    else if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.left, worldGenerator, airPixel) ||
                        GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.right, worldGenerator, airPixel))
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, scorchedRockPixel);
                    }
                }
            }
        }
    }
}
