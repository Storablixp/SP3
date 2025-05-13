using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Terrain Mutator", menuName = "Scriptable Objects/World Mutator/Terrain")]
public class TerrainMutator : WorldMutatorSO
{
    [Header("Layer Settings")]
    [Range(0.001f, 1f)] public float airLayerThreshold = 0.9f;
    [Range(0.001f, 1f)] public float surfaceLayerThreshold = 0.75f;
    [Range(0.001f, 1f)] public float stoneThreshold = 0.30f;

    [Header("Other Settings")]
    public Perlin2DSettings noiseSettings;
    [Range(0, 100)] public int heightVariationStrength = 50;

    [Header("Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO dirtPixel;
    [SerializeField] private PixelSO sandPixel;
    [SerializeField] private PixelSO snowPixel;
    [SerializeField] private PixelSO deepStonePixel;
    [SerializeField] private PixelSO hollowPixel;
    [SerializeField] private PixelSO stonePixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);
                float yMod = arrayY + (noiseValue - 0.5f) * 2f * heightVariationStrength;

                PixelSO pixelToAdd;
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                if (yMod > worldSize.y * airLayerThreshold)
                    pixelToAdd = airPixel;
                else if (yMod > worldSize.y * surfaceLayerThreshold)
                {
                    if (pixelInstance.Temperature == 2)
                    {
                        pixelToAdd = sandPixel;
                    }
                    else if (pixelInstance.Temperature == -1)
                    {
                        pixelToAdd = snowPixel;
                    }
                    else
                    {
                        pixelToAdd = dirtPixel;
                    }
                }
                    
                else if (yMod > worldSize.y * stoneThreshold)
                {
                    if (pixelInstance.Pixel == hollowPixel)
                    {
                        continue;
                    }
                    else pixelToAdd = stonePixel;
                }
                else pixelToAdd = deepStonePixel;

                worldGenerator.ChangePixel(arrayX, arrayY, pixelToAdd);
            }
        }

        yield return null;
    }
}
