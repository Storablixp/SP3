using UnityEngine;
using System.Collections;
using UnityEditorInternal.VersionControl;

[CreateAssetMenu(fileName = "Terrain Mutator", menuName = "Scriptable Objects/World Mutator/Terrain")]
public class TerrainMutator : WorldMutatorSO
{
    //[Header("Optional Mutators")]
    //[SerializeField] private TemperatureMutator temperatureMutator;
    //[SerializeField] private HumidityMutator humidityMutator;

    [Header("Settings")]
    public Perlin2DSettings noiseSettings;
    [Range(0, 100)] public int heightVariationStrength = 50;
    [Range(0.001f, 1f)] public float airThreshold = 0.9f;
    [Range(0.001f, 1f)] public float dirtThreshold = 0.75f;
    [Range(0.001f, 1f)] public float stoneThreshold = 0.30f;

    [Header("Pixels")]
    public PixelSO hollowPixel;
    public PixelSO airPixel;
    public PixelSO dirtPixel;
    public PixelSO stonePixel;
    public PixelSO deepStonePixel;

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

                if (yMod > worldSize.y * airThreshold)
                    pixelToAdd = airPixel;
                else if (yMod > worldSize.y * dirtThreshold)
                    pixelToAdd = dirtPixel;
                else if (yMod > worldSize.y * stoneThreshold)
                {
                    if (pixels[arrayX, arrayY].pixel == hollowPixel)
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
