using UnityEngine;
using UnityEngine.UI;

public class PerlinNoise2DVisualizer : MonoBehaviour
{
    [Range(0, 100)] public int heightVariationStrength = 50;

    [Header("Settings")]
    private RawImage image;
    private Texture2D texture;
    [SerializeField] private int seed;
    [SerializeField] private Vector2Int imageSize;
    [SerializeField] private Perlin2DSettings depthNoise;
    [SerializeField] private Perlin2DSettings coreNoise;
    [SerializeField] private bool saveImage;
    [SerializeField] private bool oneTimeGeneration;
    [SerializeField] private bool oneTimeRandom;
    private enum ViewType { defaultView, layerView, temperatureView, humidityView }
    [SerializeField] private ViewType view;

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
                if (view == ViewType.layerView)
                {
                    float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, depthNoise);
                    float value = noiseValue;

                    Color colorToAdd;



                    if (value > 0.9f)
                        colorToAdd = new Color(1f, 1f, 1f);
                    else if (value > 0.8f)
                        colorToAdd = new Color(0.9f, 0.9f, 0.9f);
                    else if (value > 0.7f)
                        colorToAdd = new Color(0.8f, 0.8f, 0.8f);
                    else if (value > 0.6f)
                        colorToAdd = new Color(0.7f, 0.7f, 0.7f);
                    else if (value > 0.5f)
                        colorToAdd = new Color(0.6f, 0.6f, 0.6f);
                    else if (value > 0.4f)
                        colorToAdd = new Color(0.5f, 0.5f, 0.5f);
                    else
                        colorToAdd = Color.red;


                    //if (value > 0.9f)
                    //    colorToAdd = new Color(1f, 1f, 1f);
                    //else if (value > 0.8f)
                    //    colorToAdd = new Color(0.9f, 0.9f, 0.9f);
                    //else if (value > 0.7f)
                    //    colorToAdd = new Color(0.8f, 0.8f, 0.8f);
                    //else if (value > 0.6f)
                    //    colorToAdd = new Color(0.7f, 0.7f, 0.7f);
                    //else if (value > 0.5f)
                    //    colorToAdd = new Color(0.6f, 0.6f, 0.6f);
                    //else if (value > 0.4f)
                    //    colorToAdd = new Color(0.5f, 0.5f, 0.5f);
                    //else if (value > 0.3f)
                    //    colorToAdd = new Color(0.4f, 0.4f, 0.4f);
                    //else if (value > 0.2f)
                    //    colorToAdd = new Color(0.3f, 0.3f, 0.3f);
                    //else if (value > 0.1f)
                    //    colorToAdd = new Color(0.2f, 0.2f, 0.2f);
                    //else
                    //    colorToAdd = new Color(0.1f, 0.1f, 0.1f);

                    texture.SetPixel(x, y, colorToAdd);
                }
                else if (view == ViewType.temperatureView)
                {
                    float depthNoiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, depthNoise);
                    float depthbaseTemperature = depthNoiseValue;
                    float depthFactor = (float)y / (imageSize.y - 1);
                    float depthTemperature = depthbaseTemperature + depthFactor;

                    //Radial gradient from center
                    float coreNoiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, coreNoise);
                    float coreBaseTemperature = coreNoiseValue;
                    float centerX = imageSize.x / 2f;
                    float centerY = imageSize.y / 2f;
                    float dx = x - centerX;
                    float dy = y - centerY;
                    float distanceFromCenter = Mathf.Sqrt(dx * dx + dy * dy);
                    float maxDistance = Mathf.Sqrt(centerX * centerX + centerY * centerY);
                    float radialFactor = 1f - Mathf.InverseLerp(0f, maxDistance, distanceFromCenter);
                    float coreTemperature = coreBaseTemperature * radialFactor;

                    depthTemperature = Mathf.Clamp01(depthTemperature);
                    coreTemperature = Mathf.Clamp01(coreTemperature);

                    float finalTemperature = Mathf.Lerp(depthTemperature, coreTemperature, 0.5f);

                    Color tempColor;
                    float t = Mathf.InverseLerp(0f, 1f, finalTemperature);

                    if (t < 0.33f)
                    {
                        tempColor = Color.Lerp(Color.blue, Color.green, t / 0.33f);
                    }
                    else if (t < 0.66f)
                    {
                        tempColor = Color.Lerp(Color.green, Color.yellow, (t - 0.33f) / 0.33f);
                    }
                    else
                    {
                        tempColor = Color.Lerp(Color.yellow, Color.red, (t - 0.66f) / 0.34f);
                    }

                    texture.SetPixel(x, y, tempColor);
                }
                else if (view == ViewType.humidityView)
                {
                    float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, depthNoise);
                    float t = Mathf.InverseLerp(0f, 1f, noiseValue);

                    Color tempColor;
                    if (t < 0.25f)
                    {
                        tempColor = Color.Lerp(new Color(0.96f, 0.87f, 0.70f), new Color(0.71f, 0.53f, 0.38f), t / 0.25f);
                    }
                    else if (t < 0.5f)
                    {
                        tempColor = Color.Lerp(new Color(0.71f, 0.53f, 0.38f), new Color(0.56f, 0.93f, 0.56f), (t - 0.25f) / 0.25f);
                    }
                    else if (t < 0.75f)
                    {
                        tempColor = Color.Lerp(new Color(0.56f, 0.93f, 0.56f), Color.green, (t - 0.5f) / 0.25f);
                    }
                    else
                    {
                        tempColor = Color.Lerp(Color.green, new Color(0.0f, 0.39f, 0.0f), (t - 0.75f) / 0.25f);
                    }

                    texture.SetPixel(x, y, tempColor);
                }
                else
                {
                    float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, depthNoise);
                    texture.SetPixel(x, y, new Color(noiseValue, noiseValue, noiseValue));
                }
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
