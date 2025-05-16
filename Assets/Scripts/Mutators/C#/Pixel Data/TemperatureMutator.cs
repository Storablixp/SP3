using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Temperature Mutator", menuName = "Scriptable Objects/World Mutator/Pixel Data/Temperature")]
public class TemperatureMutator : WorldMutatorSO
{
   
    [Header("Settings")]
    [SerializeField] private Perlin2DSettings noiseSettings;
    [SerializeField, Range(0, 100)] private int heightVariationStrength = 50;
    [SerializeField] private List<int> temperatureList = new();
    private int startIndex;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();
        
        startIndex = Random.Range(0, temperatureList.Count);
        List<int> newTemperatureList = new List<int>();
        
        for (int i = startIndex; i < temperatureList.Count; i++)
        {
            newTemperatureList.Add(temperatureList[i]);
        }
        for (int i = 0; i < startIndex; i++)
        {
            newTemperatureList.Add(temperatureList[i]);
        }

        float biomeSize = 1f / newTemperatureList.Count;

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(arrayX, arrayY, WorldGenerator.XOffset, WorldGenerator.YOffset, noiseSettings);
                float xMod = arrayX + (noiseValue - 0.5f) * 2f * heightVariationStrength;

                bool didBreak = false;
                for (int i = 1; i < newTemperatureList.Count; i++)
                {
                    if (xMod > worldSize.x * (1 - (biomeSize * i)))
                    {
                        pixelInstance.Temperature = newTemperatureList[i - 1];
                        didBreak = true;
                        break;
                    }
                }

                if (!didBreak)
                {
                    pixelInstance.Temperature = newTemperatureList[newTemperatureList.Count - 1];
                }

                pixels[arrayX, arrayY] = pixelInstance;
            }
        }
        yield return null;
    }

    //[System.Serializable]
    //public struct TemperatureArea
    //{
    //    public string Name;
    //    [Range(32, 256)] public int MinLength;
    //    [Range(32, 256)] public int MaxLength;
    //}
}
