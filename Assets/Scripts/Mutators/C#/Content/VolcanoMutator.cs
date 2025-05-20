using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Volcano Mutator", menuName = "Scriptable Objects/World Mutator/Content/Volcano")]

public class VolcanoMutator : WorldMutatorSO
{
    [Header("Settings")]
    [SerializeField] private TextAsset textFile;

    [Header("Other Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO lavaPixel;
    [SerializeField] private PixelSO volcanicRockPixel;
    [SerializeField] private PixelSO volcanoPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();
        Vector2Int centerRock = FindCenterVolcanicRock(worldSize, pixels);

        PlaceVolcano(centerRock.x - 8, centerRock.y + 8, pixels);
        CleanNearbyVolcanicRock(worldSize, pixels);

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

    private void PlaceVolcano(int arrayX, int arrayY, PixelInstance[,] pixels)
    {
        string[] lines = textFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int y = 16; y >= 0; y--)
        {
            string line = lines[y];
            for (int x = 0; x < 17; x++)
            {
                switch (line[x])
                {
                    case '1':
                        if (pixels[arrayX + x, arrayY - y].Pixel != volcanicRockPixel)
                        {
                            if (!GlobalNeighborCheckFucntions.SimpleCheck(arrayX + x, arrayY - y, Vector2Int.up, worldGenerator, volcanicRockPixel) &&
                                !GlobalNeighborCheckFucntions.SimpleCheck(arrayX + x, arrayY - y, Vector2Int.up, worldGenerator, lavaPixel))
                            {
                                worldGenerator.ChangePixel(arrayX + x, arrayY - y, volcanoPixel);
                            }
                        }
                        break;
                    case '2':
                        worldGenerator.ChangePixel(arrayX + x, arrayY - y, lavaPixel);
                        break;
                }
            }
        }
    }

    private void CleanNearbyVolcanicRock(Vector2Int worldSize, PixelInstance[,] pixels)
    {
        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                if (pixelInstance.Pixel == volcanicRockPixel &&
                    GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, airPixel) &&
                    GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 1, lavaPixel, 1) &&
                    GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 1, volcanoPixel, 0))
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, volcanoPixel);
                }
            }
        }

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                if (pixelInstance.Pixel == volcanoPixel)
                {
                    if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.down, worldGenerator, volcanoPixel))
                    {
                        continue;
                    }

                    int i = 0;
                    while (!GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY - i, Vector2Int.down, worldGenerator, lavaPixel))
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY - (i + 1), lavaPixel);
                        i++;
                    }
                }

            }
        }
    }
}
