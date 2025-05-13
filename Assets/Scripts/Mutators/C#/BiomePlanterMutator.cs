using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Biome Planter Mutator", menuName = "Scriptable Objects/World Mutator/Biome/Planter")]
public class BiomePlanterMutator : WorldMutatorSO
{
    [Header("Settings")]
    //[Range(0.0f, 1.0f), SerializeField] private float WarmBiomeThreshold;
    //[Range(0.0f, 1.0f), SerializeField] private float ColdBiomeThreshold;

    [Header("Pixels")]
    [SerializeField] private PixelSO VeryWarmPixel;
    [SerializeField] private PixelSO WarmPixel;
    [SerializeField] private PixelSO HumidPixel;
    [SerializeField] private PixelSO ColdPixel;
    [SerializeField] private PixelSO VeryColdPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];
                
                if (pixel.SunlightLevel == 2)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, VeryWarmPixel);
                }
                else if (pixel.SunlightLevel == 1)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, WarmPixel);
                }
                else if (pixel.SunlightLevel == 0)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, HumidPixel);
                }
                else if (pixel.SunlightLevel == -1)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, ColdPixel);
                }
                else if (pixel.SunlightLevel == -2)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, VeryColdPixel);
                }
            }
        }

        yield return null;
    }
}
