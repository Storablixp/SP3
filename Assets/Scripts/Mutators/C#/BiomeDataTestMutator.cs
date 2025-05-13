using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Biome Planter Mutator", menuName = "Scriptable Objects/World Mutator/Testing/Biome Data")]
public class BiomeDataTestMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO VeryHighValue;
    [SerializeField] private PixelSO HighValue;
    [SerializeField] private PixelSO ModerateValue;
    [SerializeField] private PixelSO LowValue;
    [SerializeField] private PixelSO VeryLowValue;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];
                
                if (pixel.Wetness == 2)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, VeryHighValue);
                }
                else if (pixel.Wetness == 1)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, HighValue);
                }
                else if (pixel.Wetness == 0)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, ModerateValue);
                }
                else if (pixel.Wetness == -1)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, LowValue);
                }
                else if (pixel.Wetness == -2)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, VeryLowValue);
                }
            }
        }

        yield return null;
    }
}
