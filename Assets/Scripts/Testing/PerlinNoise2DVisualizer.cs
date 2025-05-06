using UnityEngine;
using UnityEngine.UI;

public class PerlinNoise2DVisualizer : MonoBehaviour
{
    private RawImage image;
    private Texture2D texture;
    [SerializeField] private int seed;
    [SerializeField] private Vector2Int imageSize;
    [SerializeField] private Perlin2DSettings noiseSettings;
    [SerializeField] private bool saveImage;
    [SerializeField] private bool oneTimeGeneration;
    private enum ViewType { defaultView, temperatureView, humidityView }
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
        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

        for (int y = imageSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < imageSize.x; x++)
            {
                if (view == ViewType.defaultView)
                {
                    float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, noiseSettings);
                    texture.SetPixel(x, y, new Color(noiseValue, noiseValue, noiseValue));
                }
                else if (view == ViewType.temperatureView)
                {
                    float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, noiseSettings);
                    float depthFactor = (float)y / (imageSize.y - 1);
                    float finalTemperature = noiseValue;

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
                else if(view == ViewType.humidityView)
                {
                    float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, noiseSettings);
                    float t = Mathf.InverseLerp(0f, 1f, noiseValue);

                    Color tempColor;
                    if (t < 0.25f)
                    {
                        // 0% - 25%: Beige to Light Brown
                        tempColor = Color.Lerp(new Color(0.96f, 0.87f, 0.70f), new Color(0.71f, 0.53f, 0.38f), t / 0.25f);
                    }
                    else if (t < 0.5f)
                    {
                        // 25% - 50%: Light Brown to Light Green
                        tempColor = Color.Lerp(new Color(0.71f, 0.53f, 0.38f), new Color(0.56f, 0.93f, 0.56f), (t - 0.25f) / 0.25f);
                    }
                    else if (t < 0.75f)
                    {
                        // 50% - 75%: Light Green to Green
                        tempColor = Color.Lerp(new Color(0.56f, 0.93f, 0.56f), Color.green, (t - 0.5f) / 0.25f);
                    }
                    else
                    {
                        // 75% - 100%: Green to Dark Green
                        tempColor = Color.Lerp(Color.green, new Color(0.0f, 0.39f, 0.0f), (t - 0.75f) / 0.25f);
                    }

                    texture.SetPixel(x, y, tempColor);
                }
            }
        }

        texture.Apply();
        image.texture = texture;

        if (saveImage && oneTimeGeneration)
        {
            System.IO.File.WriteAllBytes("Assets/Scripts/Testing/NoiseTexture.png", texture.EncodeToPNG());
        }
    }
}
