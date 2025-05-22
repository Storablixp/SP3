using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Freeze Water", menuName = "Scriptable Objects/World Mutator/Cleaning/Freeze Water")]
public class FreezeWaterMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO IcePixel;
    [SerializeField] private PixelSO SnowPixel;
    [SerializeField] private PixelSO WaterPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {

                PixelInstance pixel = pixels[arrayX, arrayY];

                if (pixel.Pixel == WaterPixel)
                {
                    if (GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 2, IcePixel, 0) ||
                        GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 1, SnowPixel, 0))
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, IcePixel);
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
}
