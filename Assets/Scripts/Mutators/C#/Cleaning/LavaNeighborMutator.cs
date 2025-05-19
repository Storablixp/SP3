using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Lava Neighbor Mutator", menuName = "Scriptable Objects/World Mutator/Cleaning/Lava Neighbor")]
public class LavaNeighborMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO deepstonePixel;
    [SerializeField] private PixelSO hollowPixel;
    [SerializeField] private PixelSO lavaPixel;
    [SerializeField] private PixelSO obsidianPixel;
    [SerializeField] private PixelSO sandPixel;
    [SerializeField] private PixelSO stonePixel;
    [SerializeField] private PixelSO scorchedRockPixel;
    [SerializeField] private PixelSO waterPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];

                if (pixel.Pixel == lavaPixel)
                {
                    if (GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 2, sandPixel, 1) ||
                        GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 2, stonePixel, 1))
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, scorchedRockPixel);
                    }
                    else if(GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 2, hollowPixel, 1))
                    {
                        if(pixel.Depth < 0)
                        {
                            worldGenerator.ChangePixel(arrayX, arrayY, deepstonePixel);
                        }
                        else worldGenerator.ChangePixel(arrayX, arrayY, scorchedRockPixel);
                    }
                }
                if (pixel.Pixel == waterPixel)
                {
                    if (GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 2, lavaPixel, 0))
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, obsidianPixel);
                    }
                }
            }
        }

        yield return null;
    }
}
