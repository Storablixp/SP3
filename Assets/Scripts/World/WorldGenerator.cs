using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldGenerator : MonoBehaviour
{
    [Header("World Mutators")]
    [SerializeField] private List<WorldMutatorSO> biomeMutators = new();
    [SerializeField] private List<WorldMutatorSO> contentMutators = new();
    [SerializeField] private List<WorldMutatorSO> cleaningMutators = new();


    [Header("Testing")]
    [SerializeField] private bool isTesting;
    [SerializeField] private bool showChanges;
    [SerializeField] private bool differentOffsets;
    [SerializeField] private bool disableCleaningMutators;

    [Header("World Settings")]
    [SerializeField] private PixelSO defaultPixel;
    [SerializeField] private int seed;
    [SerializeField] private Vector2Int worldSize;
    private ChunkAndPlayerGenerator chunkManager;
    public static float XOffset;
    public static float YOffset;

    [Header("Components")]
    [SerializeField] private GameObject worldCanvasPrefab;
    [SerializeField] private WorldMap worldMap;

    [Header("Other Variables")]
    private PixelInstance[,] pixels;
    public Dictionary<Vector2Int, TileInstance> Tiles = new();
    private Texture2D worldTexture;
    private RawImage rawImage;

    private void OnValidate()
    {
        if (worldSize.x <= 0) worldSize.x = 1;
        if (worldSize.y <= 0) worldSize.y = 1;
    }

    private void Awake()
    {
        Random.InitState(seed);

        foreach (WorldMutatorSO mutator in biomeMutators)
        {
            mutator.SetUp(this, worldSize);
        }

        foreach (WorldMutatorSO mutator in contentMutators)
        {
            mutator.SetUp(this, worldSize);
        }


        if (!disableCleaningMutators)
        {
            foreach (WorldMutatorSO mutator in cleaningMutators)
            {
                mutator.SetUp(this, worldSize);
            }
        }

        chunkManager = GetComponent<ChunkAndPlayerGenerator>();
    }

    void Start()
    {
        if (worldSize.x % 2 == 1) worldSize.x++;
        if (worldSize.y % 2 == 1) worldSize.y++;

        XOffset = Random.Range(-100000f, 100000f);
        YOffset = Random.Range(-100000f, 100000f);

        pixels = new PixelInstance[worldSize.x, worldSize.y];
        StartCoroutine(nameof(GenerateWorld));
    }

    private IEnumerator GenerateWorld()
    {
        yield return StartCoroutine(FillWithAir());


        foreach (WorldMutatorSO mutator in biomeMutators)
        {
            yield return StartCoroutine(mutator.ApplyMutator(worldSize));
        }

        foreach (WorldMutatorSO mutator in contentMutators)
        {
            yield return StartCoroutine(mutator.ApplyMutator(worldSize));
        }

        if (!disableCleaningMutators)
        {
            foreach (WorldMutatorSO mutator in cleaningMutators)
            {
                yield return StartCoroutine(mutator.ApplyMutator(worldSize));
            }
        }

        GenerateTexture();
        worldMap.SetUp(worldSize, rawImage.texture);

        if (isTesting)
        {
            if (!showChanges) yield break;

            pixels = new PixelInstance[worldSize.x, worldSize.y];
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
            rawImage.gameObject.SetActive(false);
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

                if (pixels[x, y].Pixel != null)
                {
                    color = pixels[x, y].Pixel.Color;
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
        if (pixels[xCoord, yCoord].Pixel == null || pixels[xCoord, yCoord].Pixel.Unchangeable) //Return if pixel can't be changed.
        {
            return;
        }

        pixels[xCoord, yCoord].Pixel = pixel;
    }

    public void TurnToAir(int xCoord, int yCoord) => pixels[xCoord, yCoord].Pixel = defaultPixel;

    public PixelInstance[,] RetrievePixels() => pixels;

    public void AddTile(Vector2Int position, WorldTile tile)
    {
        TileInstance newInstance = new TileInstance();
        newInstance.Tile = tile;
        Tiles.Add(position, newInstance);
    }

    public void ReplaceTile(Vector2Int position, WorldTile tile)
    {
        if (Tiles.TryGetValue(position, out TileInstance instance))
        {
            instance.Tile = tile;
            Tiles[position] = instance;
        }
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
