using UnityEngine;
using UnityEngine.UI;

public class PerlinNoise2DVisualizer : MonoBehaviour
{
    [Header("Settings")]
    private RawImage image;
    private Texture2D texture;
    [SerializeField] private int seed;
    [SerializeField] private Vector2Int imageSize;
    [SerializeField] private Perlin2DSettings depthNoise;
    [SerializeField] private Perlin2DSettings coreNoise;
    [SerializeField] private Perlin2DSettings seasonsNoise;
    [SerializeField] private bool oneTimeGeneration;
    [SerializeField] private bool oneTimeRandom;
    [Range(0, 100), SerializeField] private int heightVariationStrength = 50;

    private void Start()
    {
        Random.InitState(seed);
        image = GetComponent<RawImage>();

        texture = new Texture2D(imageSize.x, imageSize.y);

        if (oneTimeGeneration)
        {
            GenerateNoise();
        }
        else InvokeRepeating(nameof(GenerateNoise), 0, 0.1f);
    }

    private void GenerateNoise()
    {
        float xOffset = 0;
        float yOffset = 0;

        if (!oneTimeRandom)
        {
            xOffset = Random.Range(-10000f, 10000f);
            yOffset = Random.Range(-10000f, 10000f);
        }


        for (int y = imageSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < imageSize.x; x++)
            {
                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, WorldGenerator.XOffset, WorldGenerator.YOffset, seasonsNoise);
                float xMod = x + noiseValue * heightVariationStrength;
                float yMod = y + noiseValue * heightVariationStrength;

                float xNormalized = Mathf.Clamp01(xMod / imageSize.x);
                float yNormalized = Mathf.Clamp01(yMod / imageSize.y);

                float value = (xNormalized + yNormalized) / 2f;

                Color tempColor;

                if (value > 0.75f)
                {
                    tempColor = new Color(1f, 1f, 1f);
                }
                else if (value > 0.5f)
                {
                    tempColor = new Color(0.75f, 0.75f, 0.75f);
                }
                else if (value > 0.25f)
                {
                    tempColor = new Color(0.5f, 0.5f, 0.5f);
                }
                else tempColor = new Color(0.25f, 0.25f, 0.25f);


                //float depthNoiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, depthNoise);
                //float depthbaseTemperature = depthNoiseValue;
                //float depthFactor = (float)y / (imageSize.y - 1);
                //float depthTemperature = depthbaseTemperature + depthFactor;

                ////Radial gradient from center
                //float coreNoiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, coreNoise);
                //float coreBaseTemperature = coreNoiseValue;
                //float centerX = imageSize.x / 2f;
                //float centerY = imageSize.y / 2f;
                //float dx = x - centerX;
                //float dy = y - centerY;
                //float distanceFromCenter = Mathf.Sqrt(dx * dx + dy * dy);
                //float maxDistance = Mathf.Sqrt(centerX * centerX + centerY * centerY);
                //float radialFactor = 1f - Mathf.InverseLerp(0f, maxDistance, distanceFromCenter);
                //float coreTemperature = coreBaseTemperature * radialFactor;

                //depthTemperature = Mathf.Clamp01(depthTemperature);
                //coreTemperature = Mathf.Clamp01(coreTemperature);

                //float finalTemperature = Mathf.Lerp(depthTemperature, coreTemperature, 0.5f);

                //Color tempColor;
                //float t = Mathf.InverseLerp(0f, 1f, finalTemperature);

                //if (t < 0.33f)
                //{
                //    tempColor = Color.Lerp(Color.blue, Color.green, t / 0.33f);
                //}
                //else if (t < 0.66f)
                //{
                //    tempColor = Color.Lerp(Color.green, Color.yellow, (t - 0.33f) / 0.33f);
                //}
                //else
                //{
                //    tempColor = Color.Lerp(Color.yellow, Color.red, (t - 0.66f) / 0.34f);
                //}

                texture.SetPixel(x, y, tempColor);
            }
        }

        texture.Apply();
        image.texture = texture;

        //if (saveImage && oneTimeGeneration)
        //{
        //    System.IO.File.WriteAllBytes("Assets/Scripts/Testing/NoiseTexture.png", texture.EncodeToPNG());
        //}
    }
}
