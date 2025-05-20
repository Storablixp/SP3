using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Cloud Mutator", menuName = "Scriptable Objects/World Mutator/Content/Cloud")]

public class CloudMutator : WorldMutatorSO
{
    [Header("Settings")]
    [SerializeField] private TextAsset textFile;

    [Header("Other Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO lavaPixel;
    [SerializeField] private PixelSO volcanicRockPixel;
    [SerializeField] private PixelSO scorchedRockPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();
        Vector2Int centerRock = FindCenterVolcanicRock(worldSize, pixels);
        PlaceCloud(centerRock.x - 4, centerRock.y + 12);

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

    private void PlaceCloud(int arrayX, int arrayY)
    {
        string[] lines = textFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int y = 15; y >= 0; y--)
        {
            string line = lines[y];
            for (int x = 0; x < 16; x++)
            {
                switch (line[x])
                {
                    case '1':
                        worldGenerator.ChangePixel(arrayX + x, arrayY - y, scorchedRockPixel);
                        break;
                    case '2':
                        worldGenerator.ChangePixel(arrayX + x, arrayY - y, lavaPixel);
                        break;
                }
            }
        }
    }
}
