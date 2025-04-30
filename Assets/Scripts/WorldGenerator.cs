using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class WorldGenerator : MonoBehaviour
{
    [Header("Other")]
    private PixelSO[,] pixels;

    [Header("World Settings")]
    private Texture2D worldTexture;
    public static float XOffset;
    public static float YOffset;
    [SerializeField] private int seed;
    [SerializeField] private Vector2Int worldSize;

    [Header("Components")]
    private RectTransform rectTransform;
    private RawImage rawImage;
    [SerializeField] private WorldMutatorSO[] worldMutators;
    [SerializeField] private WorldBuilder worldBuilder;

    [Header("Visualization")]
    [SerializeField] private int tilesPerFrame = 100000;
    private int tileCounter;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();

        foreach (WorldMutatorSO mutator in worldMutators)
        {
            mutator.SetUp(this, worldSize);
        }
    }

    void Start()
    {
        Random.InitState(seed);
        XOffset = Random.Range(-100000f, 100000f);
        YOffset = Random.Range(-100000f, 100000f);
        
        pixels = new PixelSO[worldSize.x, worldSize.y];
        worldTexture = new Texture2D(worldSize.x, worldSize.y);
        rectTransform.sizeDelta = worldSize;

        StartCoroutine(nameof(GenerateWorld));
    }

    private IEnumerator GenerateWorld()
    {

        foreach (WorldMutatorSO mutator in worldMutators)
        {
            ResetCounterValues();
            yield return StartCoroutine(mutator.ApplyMutator(worldSize));
            UpdateProgressbar();
        }

        ColorPixels();
        Debug.Log(Time.realtimeSinceStartup);
    }

    public void ChangePixel(int xCoord, int yCoord, PixelSO pixel) => pixels[xCoord, yCoord] = pixel;
    public PixelSO[,] RetrievePixels() => pixels;

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

        worldTexture.filterMode = FilterMode.Point;
        worldTexture.Apply();

        rawImage.texture = worldTexture;

        worldBuilder.BuildWorld(worldSize, pixels);
    }

    public void ResetCounterValues()
    {
        tileCounter = 0;
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

    public bool IsInBounds(int arrayX, int arrayY)
    {
        if (arrayX >= 0 && arrayX < worldSize.x && arrayY >= 0 && arrayY < worldSize.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
