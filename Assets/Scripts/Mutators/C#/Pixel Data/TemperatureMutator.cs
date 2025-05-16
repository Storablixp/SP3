using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Temperature Mutator", menuName = "Scriptable Objects/World Mutator/Pixel Data/Temperature")]
public class TemperatureMutator : WorldMutatorSO
{
    [SerializeField] private List<int> temperatures = new();
    [SerializeField, Min(16)] private int temperatureAreas;
    [Header("Settings")]
    [SerializeField] private Perlin2DSettings noiseSettings;
    [SerializeField, Range(0, 100)] private int heightVariationStrength = 50;
    //[SerializeField] private List<int> temperatureList = new();
    private int startIndex;

    public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        base.SetUp(worldGenerator, worldSize);
        temperatures.Clear();

        //Fortsätt här.
        while (true)
        {
            int currentValue = 0;
            temperatures.Clear();

            for (int i = 0; i < temperatureAreas; ++i)
            {
                if (i < 3)
                {
                    temperatures.Add(0);
                }
                else if (i >= temperatureAreas - 3)
                {
                    temperatures.Add(0);
                }
                else if (i == temperatureAreas - 4)
                {
                    if (temperatures[temperatureAreas - 5] == 2)
                    {
                        temperatures.Add(1);
                    }
                    else if (temperatures[temperatureAreas - 5] == -2)
                    {
                        temperatures.Add(-1);
                    }
                    else temperatures.Add(0);
                }

                else
                {
                    int nr = Random.Range(1, 11);
                    int valueToAdd;
                    if (nr > 6)
                    {
                        valueToAdd = 0;
                    }
                    else if (nr > 3 && nr <= 6)
                    {
                        valueToAdd = 1;
                    }
                    else
                    {
                        valueToAdd = -1;
                    }

                    if (currentValue + valueToAdd > 2)
                    {
                        currentValue = 1;
                    }
                    else if (currentValue + valueToAdd < -2)
                    {
                        currentValue = -1;
                    }
                    else
                    {
                        currentValue += valueToAdd;
                    }

                    temperatures.Add(currentValue);
                }
            }

            if (temperatures.Contains(2) && temperatures.Contains(-2)) break;
        }
    }

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        startIndex = Random.Range(0, temperatures.Count);
        List<int> newTemperatureList = new List<int>();

        for (int i = startIndex; i < temperatures.Count; i++)
        {
            newTemperatureList.Add(temperatures[i]);
        }
        for (int i = 0; i < startIndex; i++)
        {
            newTemperatureList.Add(temperatures[i]);
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
