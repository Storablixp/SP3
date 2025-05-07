using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Spaghetti Cave Mutator", menuName = "Scriptable Objects/World Mutator/Cave System/Spaghetti Cave")]
public class SpaghettiCaveMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO hollowPixel;
    [SerializeField] private PixelSO stonePixel;

    [Header("Settings")]
    [SerializeField] private Vector2 holeThreshold = new Vector2(0.2f, 0.8f);
    [SerializeField] private Perlin2DSettings noiseScettings;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelSO[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                if (pixels[arrayX, arrayY] == stonePixel)
                {
                    float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseScettings);

                    if (noiseValue > holeThreshold.x && noiseValue < holeThreshold.y)
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, hollowPixel);
                    }
                }
            }
        }

        yield return null;
    }
}
