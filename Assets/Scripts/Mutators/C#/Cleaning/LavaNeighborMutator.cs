using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Lava Neighbor Mutator", menuName = "Scriptable Objects/World Mutator/Cleaning/Lava Neighbor")]
public class LavaNeighborMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO hollowPixel;
    [SerializeField] private PixelSO lavaPixel;
    [SerializeField] private PixelSO sandPixel;
    [SerializeField] private PixelSO stonePixel;
    [SerializeField] private PixelSO scorchedRockPixel;

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
                    if (GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 2, sandPixel, 0) ||
                        GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 2, stonePixel, 0) ||
                        GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 2, hollowPixel, 0))
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, scorchedRockPixel);
                    }
                }
            }
        }

        yield return null;
    }
}
