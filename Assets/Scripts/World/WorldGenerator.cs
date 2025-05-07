using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private PixelSO airPixel;
    private PixelSO[,] pixels;

    [Header("Testing")]
    [SerializeField] private bool testing;
    [SerializeField] private bool differentOffsets;

    [Header("World Settings")]
    [SerializeField] private int seed;
    [SerializeField] private Vector2Int worldSize;
    [SerializeField] private List<WorldMutatorSO> worldMutators = new();
    private ChunkAndPlayerGenerator chunkManager;
    public static float XOffset;
    public static float YOffset;

    [Header("Visualization")]
    [SerializeField] private int tilesPerFrame = 100000;
    [SerializeField] private GameObject worldCanvasPrefab;
    private int tileCounter;
    private Texture2D worldTexture;
    private RawImage rawImage;

    private void OnValidate()
    {
        if (worldSize.x <= 0) worldSize.x = 1;
        if (worldSize.y <= 0) worldSize.y = 1;
    }

    private void Awake()
    {
        foreach (WorldMutatorSO mutator in worldMutators)
        {
            mutator.SetUp(this, worldSize);
        }

        chunkManager = GetComponent<ChunkAndPlayerGenerator>();
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
        yield return StartCoroutine(FillWithAir());


        foreach (WorldMutatorSO mutator in worldMutators)
        {
            //ResetCounterValues();
            yield return StartCoroutine(mutator.ApplyMutator(worldSize));
            //UpdateProgressbar();
        }


        if (testing)
        {
            GenerateTexture();

            pixels = new PixelSO[worldSize.x, worldSize.y];
            if (differentOffsets)
            {
                XOffset = Random.Range(-100000f, 100000f);
                YOffset = Random.Range(-100000f, 100000f);
            }
            StartCoroutine(GenerateWorld());
            yield break;
        }
        else
        {
            StartCoroutine(chunkManager.SpawnChunksAndPlayer(worldSize, pixels));
        }

        Debug.Log(Time.realtimeSinceStartup);
    }

    private IEnumerator FillWithAir()
    {
        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                TurnToAir(x, y);
            }
        }

        yield return null;
    }

    private void GenerateTexture()
    {
        if (worldTexture == null)
        {
            worldTexture = new Texture2D(worldSize.x, worldSize.y);
            GameObject childObj = Instantiate(worldCanvasPrefab, Vector3.zero, Quaternion.identity).transform.GetChild(0).gameObject;
            childObj.GetComponent<RectTransform>().sizeDelta = worldSize;
            rawImage = childObj.GetComponent<RawImage>();
        }

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

        rawImage.texture = worldTexture;
    }

    public void ChangePixel(int xCoord, int yCoord, PixelSO pixel)
    {
        if (pixels[xCoord, yCoord] != null && pixels[xCoord, yCoord].Unchangeable) //Return if pixel can't be changed.
        {
            return;
        }

        pixels[xCoord, yCoord] = pixel;

    }

    public void TurnToAir(int xCoord, int yCoord) => pixels[xCoord, yCoord] = airPixel;

    public PixelSO[,] RetrievePixels() => pixels;

    public bool CheckForMutator(WorldMutatorSO mutator)
    {
        return worldMutators.Contains(mutator);
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
