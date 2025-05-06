using UnityEngine;
using UnityEngine.UI;

public class PerlinNoise2DVisualizer : MonoBehaviour
{
    private RawImage image;
    private Texture2D texture;
    [SerializeField] private bool saveImage;
    [SerializeField] private int seed;
    [SerializeField] private Vector2Int imageSize;
    [SerializeField] private Perlin2DSettings noiseSettings;
    [SerializeField] private bool oneTimeGeneration;

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
                //float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, noiseSettings);
                //texture.SetPixel(x, y, new Color(noiseValue, noiseValue, noiseValue));

                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, noiseSettings);
                float baseTemperature = (noiseValue - 0.5f) * 2;
                float depthFactor = (float)y / (imageSize.y - 1);
                float finalTemperature = baseTemperature /*+ depthFactor*/;

                Color tempColor;
                float t = Mathf.InverseLerp(-1f, 1f, finalTemperature);

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
        }

        texture.Apply();
        image.texture = texture;

        if (saveImage && oneTimeGeneration)
        {
            System.IO.File.WriteAllBytes("Assets/Scripts/Testing/NoiseTexture.png", texture.EncodeToPNG());
        }
    }
}
