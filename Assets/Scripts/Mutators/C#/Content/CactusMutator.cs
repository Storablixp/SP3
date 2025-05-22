using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Cactus Mutator", menuName = "Scriptable Objects/World Mutator/Content/Cactus")]
public class CactusMutator : WorldMutatorSO
{
    [Header("Settings")]
    [SerializeField, Min(0)] private int cactusPlacementRate = 16;
    [SerializeField] private TextAsset textFile;

    [Header("Other Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO sandPixel;

    [Header("Cactus Pixels")]
    [SerializeField] private PixelSO cactus_1Pixel;
    [SerializeField] private PixelSO cactus_2Pixel;
    [SerializeField] private PixelSO cactus_3Pixel;

    private int pixelsBeforeCanPlace;
    public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        base.SetUp(worldGenerator, worldSize);
        pixelsBeforeCanPlace = Random.Range(0, cactusPlacementRate - 1);
    }

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];

                if (pixel.Pixel == sandPixel)
                {
                    if (pixelsBeforeCanPlace <= 0)
                    {
                        if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.right, worldGenerator, sandPixel) &&
                       GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.left, worldGenerator, sandPixel) &&
                       GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.right * 2, worldGenerator, sandPixel) &&
                       GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.left * 2, worldGenerator, sandPixel) &&
                       GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, airPixel))
                        {
                            PlaceCactus(arrayX - 6, arrayY + 16);
                            pixelsBeforeCanPlace = cactusPlacementRate;
                        }
                    }

                    if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, airPixel))
                    {
                        pixelsBeforeCanPlace--;
                    }
                }

                if (worldGenerator.UpdateProgressbar(false))
                {
                    yield return null;
                }
            }
        }
        yield return null;
    }

    private void PlaceCactus(int arrayX, int arrayY)
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
                        worldGenerator.ChangePixel(arrayX + x, arrayY - y, cactus_1Pixel);
                        break;
                    case '2':
                        worldGenerator.ChangePixel(arrayX + x, arrayY - y, cactus_2Pixel);
                        break;
                    case '3':
                        worldGenerator.ChangePixel(arrayX + x, arrayY - y, cactus_3Pixel);
                        break;
                }
            }
        }
    }
}
