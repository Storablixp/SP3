using UnityEngine;
using System.Collections;
using static UnityEditor.LightingExplorerTableColumn;

[CreateAssetMenu(fileName = "Biome Planter Mutator", menuName = "Scriptable Objects/World Mutator/Testing/Biome Data")]
public class BiomeDataTestMutator : WorldMutatorSO
{
    private enum DataType { depth, sunlight, wetness }
    [SerializeField] private DataType dataType;

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

                switch (dataType)
                {
                    case DataType.depth: DepthMap(arrayX, arrayY, pixel); break;
                    case DataType.sunlight: SunlightMap(arrayX, arrayY, pixel); break;
                    case DataType.wetness: WetnessMap(arrayX, arrayY, pixel); break;
                }
            }
        }

        yield return null;
    }

    private void DepthMap(int arrayX, int arrayY, PixelInstance pixel)
    {
        if (pixel.Depth == 2)
        {
            worldGenerator.ChangePixel(arrayX, arrayY, VeryHighValue);
        }
        else if (pixel.Depth == 1)
        {
            worldGenerator.ChangePixel(arrayX, arrayY, HighValue);
        }
        else if (pixel.Depth == 0)
        {
            worldGenerator.ChangePixel(arrayX, arrayY, ModerateValue);
        }
        else if (pixel.Depth == -1)
        {
            worldGenerator.ChangePixel(arrayX, arrayY, LowValue);
        }
        else if (pixel.Depth == -2)
        {
            worldGenerator.ChangePixel(arrayX, arrayY, VeryLowValue);
        }
    }
    private void SunlightMap(int arrayX, int arrayY, PixelInstance pixel)
    {
        if (pixel.SunlightLevel == 2)
        {
            worldGenerator.ChangePixel(arrayX, arrayY, VeryHighValue);
        }
        else if (pixel.SunlightLevel == 1)
        {
            worldGenerator.ChangePixel(arrayX, arrayY, HighValue);
        }
        else if (pixel.SunlightLevel == 0)
        {
            worldGenerator.ChangePixel(arrayX, arrayY, ModerateValue);
        }
        else if (pixel.SunlightLevel == -1)
        {
            worldGenerator.ChangePixel(arrayX, arrayY, LowValue);
        }
        else if (pixel.SunlightLevel == -2)
        {
            worldGenerator.ChangePixel(arrayX, arrayY, VeryLowValue);
        }
    }
    private void WetnessMap(int arrayX, int arrayY, PixelInstance pixel)
    {
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
