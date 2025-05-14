using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Better Quicksand", menuName = "Scriptable Objects/World Mutator/Cleaning/Better Quicksand")]
public class BetterQuicksandMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO sandPixel;
    [SerializeField] private PixelSO quicksandPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();
        MoveUp(worldSize, pixels);
        //RemoveLines(worldSize, pixels);

        yield return null;
    }

    private void MoveUp(Vector2Int worldSize, PixelInstance[,] pixels)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];
                if (pixel.Pixel != quicksandPixel ||
                    GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, airPixel)) continue;

                if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, sandPixel))
                {
                    int i = 0;
                    do
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY + 1 + i, quicksandPixel);
                        worldGenerator.ChangePixel(arrayX, arrayY + i, sandPixel);
                        i++;
                    }


                    while (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY + i, Vector2Int.up, worldGenerator, sandPixel));
                }
                else
                {
                    PixelSO pixelBeneath = pixels[arrayX, arrayY + 1].Pixel;
                    worldGenerator.ChangePixel(arrayX, arrayY, pixelBeneath);
                }
            }
        }
    }
    private void RemoveLines(Vector2Int worldSize, PixelInstance[,] pixels)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];
                if (pixel.Pixel == quicksandPixel)
                {
                    if (!GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 1, quicksandPixel, 2))
                    {
                        PixelSO pixelBeneath = pixels[arrayX - 1, arrayY].Pixel;
                        worldGenerator.ChangePixel(arrayX, arrayY, pixelBeneath);
                    }
                }
                else if (pixel.Pixel == sandPixel)
                {
                    if (!GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 1, sandPixel, 2))
                    {
                        PixelSO pixelBeneath = pixels[arrayX - 1, arrayY].Pixel;
                        worldGenerator.ChangePixel(arrayX, arrayY, pixelBeneath);
                    }
                }
            }
        }
    }

}
