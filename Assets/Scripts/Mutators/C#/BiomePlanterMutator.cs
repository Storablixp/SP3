using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Biome Planter Mutator", menuName = "Scriptable Objects/World Mutator/Biome/Planter")]
public class BiomePlanterMutator : WorldMutatorSO
{
    [Header("Settings")]
    [Range(0.0f, 1.0f), SerializeField] private float WarmBiomeThreshold;
    [Range(0.0f, 1.0f), SerializeField] private float ColdBiomeThreshold;

    [Header("Pixels")]
    [SerializeField] private PixelSO WarmPixel;
    [SerializeField] private PixelSO ColdPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];

                if (0.2f <= pixel.Temperature)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, ColdPixel);
                }
            }
        }

        yield return null;
    }
}
