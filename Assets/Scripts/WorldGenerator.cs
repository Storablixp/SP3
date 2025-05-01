using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private bool buildTilemap;
    private PixelSO[,] pixels;

    [Header("World Settings")]
    [SerializeField] private int seed;
    [SerializeField] private Vector2Int worldSize;
    [SerializeField] private WorldMutatorSO[] worldMutators;
    private ChunkManager chunkManager;
    public static float XOffset;
    public static float YOffset;

    [Header("Visualization")]
    [SerializeField] private bool DisplayTexture;
    [SerializeField] private int tilesPerFrame = 100000;
    [SerializeField] private GameObject worldCanvas;
    private int tileCounter;

    private void OnValidate()
    {
        if (worldSize.x <= 1) worldSize.x = 2;
        if (worldSize.y <= 1) worldSize.y = 2;
    }

    private void Awake()
    {
        foreach (WorldMutatorSO mutator in worldMutators)
        {
            mutator.SetUp(this, worldSize);
        }

        chunkManager = GetComponent<ChunkManager>();
    }

    void Start()
    {
        if (worldSize.x % 2 == 1) worldSize.x++;
        if (worldSize.y % 2 == 1) worldSize.y++;

        Random.InitState(seed);
        XOffset = Random.Range(-100000f, 100000f);
        YOffset = Random.Range(-100000f, 100000f);

        pixels = new PixelSO[worldSize.x, worldSize.y];

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


        if (DisplayTexture)
        {
            GenerateTexture();
        }


        if (buildTilemap)
        {
            chunkManager.GenerateMasterChunk(worldSize, pixels);
        }

        Debug.Log(Time.realtimeSinceStartup);
    }

    private void GenerateTexture()
    {
        Texture2D worldTexture = new Texture2D(worldSize.x, worldSize.y);

        GameObject childObj = Instantiate(worldCanvas, Vector3.zero, Quaternion.identity).transform.GetChild(0).gameObject;
        childObj.GetComponent<RectTransform>().sizeDelta = worldSize;

        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
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

        childObj.GetComponent<RawImage>().texture = worldTexture;
    }

    public void ChangePixel(int xCoord, int yCoord, PixelSO pixel) => pixels[xCoord, yCoord] = pixel;
    public PixelSO[,] RetrievePixels() => pixels;


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
