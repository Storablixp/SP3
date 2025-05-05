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

        if (oneTimeGeneration)
        {
            GenerateNoise();
        }
        else InvokeRepeating(nameof(GenerateNoise), 0, 1);
    }

    private void GenerateNoise()
    {
        texture = new Texture2D(imageSize.x, imageSize.y);

        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

        for (int y = imageSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < imageSize.x; x++)
            {
                float noiseValue = GlobalPerlinFunctions.SumPerlinNoise2D(x, y, xOffset, yOffset, noiseSettings);

                texture.SetPixel(x, y, new Color(noiseValue, noiseValue, noiseValue));
            }
        }

        texture.Apply();
        image.texture = texture;

        if (saveImage)
        {
            System.IO.File.WriteAllBytes("Assets/Scripts/Testing/NoiseTexture.png", texture.EncodeToPNG());
        }
    }
}
