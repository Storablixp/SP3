using System.Collections;
using UnityEngine;

public class GlobalPerlinFunctions
{
    #region 1D
    public static float PerlinNoise1D(int x, float xOffset, Perlin1DSettings noiseSettings, int chunkHeight)
    {
        float noiseValue = SumPerlinNoise1D(x, xOffset, noiseSettings);
        float noiseInRange = RangeMap(noiseValue, 0, 1, noiseSettings.noiseRangeMin * chunkHeight, chunkHeight);
        return noiseInRange;
    }

    private static float SumPerlinNoise1D(int x, float xOffset, Perlin1DSettings noiseSettings)
    {
        float amplitude = 1;
        float frequency = noiseSettings.Frequency;
        float noiseSum = 0;
        float amplitudeSum = 0;
        for (int i = 0; i < noiseSettings.Octaves; i++)
        {
            float sampleX = (x + xOffset) * frequency * noiseSettings.NoiseScale;

            noiseSum += amplitude * Mathf.PerlinNoise1D(sampleX);
            amplitudeSum += amplitude;
            amplitude *= noiseSettings.Persistence;
            frequency *= noiseSettings.Lacunarity;
        }

        float rawValue = noiseSum / amplitudeSum;
        float noiseValue = GlobalEasingFunctions.GetEasingValue(rawValue, noiseSettings.EasingFunctionModifier, noiseSettings.EasingFunctionType);
        return noiseValue;
    }

    private static float RangeMap(float inputValue, float inMin, float inMax, float outMin, float outMax)
    {
        return outMin + (inputValue - inMin) * (outMax - outMin) / (inMax - inMin);
    }
    #endregion

    #region 2D
    public static Texture2D GenerateNoiseTexture2D(int chunkWidth, int chunkHeight, float xOffset, float yOffset, Perlin2DSettings noiseSettings)
    {
        Texture2D texture = new Texture2D(chunkWidth, chunkHeight);
        for (int x = 0; x < chunkWidth; x++)
        {
            for (int y = 0; y < chunkHeight; y++)
            {
                float noiseValue = SumPerlinNoise2D(x, y, xOffset, yOffset, noiseSettings);
                Color color = new Color(noiseValue, noiseValue, noiseValue);
                texture.SetPixel(x, y, color);
            }
        }

        return texture;
    }

    public static float SumPerlinNoise2D(int x, int y, float xOffset, float yOffset, Perlin2DSettings noiseSettings)
    {
        float amplitude = 1;
        float frequency = noiseSettings.Frequency;
        float noiseSum = 0;
        float amplitudeSum = 0;
        for (int i = 0; i < noiseSettings.Octaves; i++)
        {
            float sampleX = (x + xOffset) * frequency * noiseSettings.NoiseScale;
            float sampleY = (y + yOffset) * frequency * noiseSettings.NoiseScale;

            noiseSum += amplitude * Mathf.PerlinNoise(sampleX, sampleY);
            amplitudeSum += amplitude;
            amplitude *= noiseSettings.Persistence;
            frequency *= noiseSettings.Lacunarity;
        }

        float rawValue = noiseSum / amplitudeSum;
        float noiseValue = GlobalEasingFunctions.GetEasingValue(rawValue, noiseSettings.EasingFunctionModifier, noiseSettings.EasingFunctionType);
        return noiseValue;
    }
    #endregion
}
