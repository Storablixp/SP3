using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Tree Mutator", menuName = "Scriptable Objects/World Mutator/Content/Tree")]
public class TreeMutator : WorldMutatorSO
{
    [Header("Settings")]
    [SerializeField, Min(0)] private int treePlacementRate = 16;

    [Header("Other Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO grassPixel;

    [Header("Tree Pixels")]
    [SerializeField] private PixelSO woodPixel;
    [SerializeField] private PixelSO leaf_1Pixel;
    [SerializeField] private PixelSO leaf_2Pixel;
    [SerializeField] private PixelSO leaf_3Pixel;
    [SerializeField] private PixelSO leaf_4Pixel;

    private int pixelsBeforeCanPlace;

    public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        base.SetUp(worldGenerator, worldSize);
        pixelsBeforeCanPlace = Random.Range(0, treePlacementRate - 1);
    }

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {

                PixelInstance pixel = pixels[arrayX, arrayY];

                if (pixel.Pixel == grassPixel)
                {
                    if (pixelsBeforeCanPlace <= 0)
                    {
                        if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.right, worldGenerator, grassPixel) &&
                            GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.left, worldGenerator, grassPixel))
                        {
                            PlaceTree(arrayX - 5, arrayY + 1);
                            pixelsBeforeCanPlace = treePlacementRate;
                        }
                    }

                    if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, airPixel))
                    {
                        pixelsBeforeCanPlace--;
                    }
                }
            }
        }
        yield return null;
    }

    private void PlaceTree(int arrayX, int arrayY)
    {
        for (int y = 10; y >= 0; y--)
        {
            for (int x = 0; x < 10; x++)
            {
                if (!worldGenerator.IsInBounds(arrayX + x, arrayY + y)) continue;

                if (y == 10 && x == 5)
                {
                    worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_4Pixel);
                }
                else if ((y == 9 || y == 8) && x > 3 && x < 7)
                {
                    if (x == 5)
                    {
                        worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_3Pixel);
                    }
                    else worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_4Pixel);
                }
                else if ((y == 7 || y == 6) && x > 2 && x < 8)
                {
                    if (x == 3 || x == 7)
                    {
                        worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_4Pixel);
                    }
                    else if (x > 3 && x < 6)
                    {
                        worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_3Pixel);
                    }
                    else worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_2Pixel);
                }
                else if ((y == 5 || y == 4) && x > 1 && x < 9)
                {
                    if (x == 2 || x == 8)
                    {
                        worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_4Pixel);
                    }
                    else if (x > 2 && x < 5)
                    {
                        if (y == 4)
                        {
                            if (x == 4)
                            {
                                worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_2Pixel);
                            }
                            else worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_3Pixel);
                        }
                        else worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_3Pixel);
                    }
                    else worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_2Pixel);
                }
                else if ((y == 3 || y == 2) && x > 0 && x < 10)
                {
                    if (x == 1 || x == 9)
                    {
                        worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_4Pixel);
                    }
                    else if (x > 1 && x < 4)
                    {
                        if (y == 2)
                        {
                            if (x == 3)
                            {
                                worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_2Pixel);
                            }
                            else worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_3Pixel);
                        }
                        else worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_3Pixel);
                    }
                    else worldGenerator.ChangePixel(arrayX + x, arrayY + y, leaf_2Pixel);
                }
                else if (y <= 1)
                {
                    if (x > 3 && x < 7)
                    {
                        worldGenerator.ChangePixel(arrayX + x, arrayY + y, woodPixel);
                    }
                }
            }
        }
    }
}
