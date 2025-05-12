using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Spaghetti Cave Mutator", menuName = "Scriptable Objects/World Mutator/Cave System/Spaghetti Cave")]
public class SpaghettiCaveMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO hollowPixel;
    [SerializeField] private PixelSO airPixel;

    [Header("Settings")]
    [SerializeField, Range(0.2f, 1.0f)] private float thickness;
    [SerializeField, Range(0.2f, 1.0f)] private float depth;
    [SerializeField] private Perlin2DSettings noiseScettings;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];
                if (pixelInstance.Depth < depth && pixelInstance.Pixel != airPixel)
                {
                    float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseScettings);

                    if (noiseValue < thickness)
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, hollowPixel);
                    }
                }
            }
        }

        yield return null;
    }
}
