using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WorldGenerator : MonoBehaviour
{
    private PixelSO[,] pixels;

    [Header("Generation Settings")]
    private Texture2D worldTexture;
    public static float XOffset;
    public static float YOffset;
    [SerializeField] private int seed;
    [SerializeField] private Vector2Int worldSize;

    [Header("Components")]
    private RectTransform rectTransform;
    private RawImage rawImage;
    [SerializeField] private WorldMutator terrain;

    [Header("Visualization")]
    [SerializeField] private int tilesPerFrame = 100000;
    private int tileCounter;
    int totalTiles;

    void Start()
    {
        Random.InitState(seed);
        XOffset = Random.Range(-100000f, 100000f);
        YOffset = Random.Range(-100000f, 100000f);

        pixels = new PixelSO[worldSize.x, worldSize.y];

        rawImage = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();

        worldTexture = new Texture2D(worldSize.x, worldSize.y);
        rectTransform.sizeDelta = worldSize;

        terrain.SetUp(this, worldSize);

        StartCoroutine(nameof(GenerateWorld));
    }

    private IEnumerator GenerateWorld()
    {
        ResetCounterValues();
        yield return StartCoroutine(terrain.ApplyMutator(worldSize));
        ColorPixels();
        Debug.Log(Time.realtimeSinceStartup);
    }

    public void AddPixel(int xCoord, int yCoord, PixelSO pixel) => pixels[xCoord, yCoord] = pixel;

    public void ColorPixels()
    {
        for (int x = 0; x < worldSize.x; x++)
        {
            for(int y = 0; y < worldSize.y; y++)
            {
                Color color = Color.white;

                if (pixels[x, y] != null)
                {
                    color = pixels[x, y].Color;
                }

                worldTexture.SetPixel(x, y, color);
            }
        }

        worldTexture.Apply();
        rawImage.texture = worldTexture;
    }

    public void ResetCounterValues()
    {
        tileCounter = 0;
        totalTiles = worldSize.x * worldSize.y;
    }

    public bool UpdateProgressbar()
    {
        tileCounter++;

        if (tileCounter >= tilesPerFrame)
        {
            tileCounter = 0;
            return true;
        }
        else return false;
    }
}
