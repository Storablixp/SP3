using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Freeze Water", menuName = "Scriptable Objects/World Mutator/Cleaning/Freeze Water")]
public class FreezeWaterMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO IcePixel;
    [SerializeField] private PixelSO SnowPixel;
    [SerializeField] private PixelSO WaterPixel;

    [Header("Settings")]
    [Header("Thresholds")]
    [SerializeField, Range(0, 25)] private uint snowThreshold = 2;
    [SerializeField, Range(0, 25)] private uint iceThreshold = 2;
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
                    if (GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 2, IcePixel, iceThreshold) ||
                        GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 2, SnowPixel, snowThreshold))
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, IcePixel);
                    }
                }

            }
        }

        yield return null;
    }
}
