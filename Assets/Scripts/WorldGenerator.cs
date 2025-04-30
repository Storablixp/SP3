using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WorldGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    private Texture2D worldTexture;
    private float xOffset;
    private float yOffset;
    [SerializeField] private int seed;
    [SerializeField] private Vector2Int worldSize;

    [Header("Components")]
    private RectTransform rectTransform;
    private RawImage rawImage;

    [Header("Visualization")]
    [SerializeField] private int tilesPerFrame = 100000;
    private int tileCounter;
    int totalTiles;

    void Start()
    {
        Random.InitState(seed);
        xOffset = Random.Range(-100000f, 100000f);
        yOffset = Random.Range(-100000f, 100000f);

        rawImage = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();

        worldTexture = new Texture2D(worldSize.x, worldSize.y);
        rectTransform.sizeDelta = worldSize;

        StartCoroutine(GenerateWorld());
    }

    private IEnumerator GenerateWorld()
    {
        ResetCounterValues();

        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                float noiseValue = Mathf.PerlinNoise((x + xOffset) * 0.01f, (y + yOffset) * 0.01f);
                worldTexture.SetPixel(x, y, new Color(noiseValue, noiseValue, noiseValue));

                if (UpdateProgressbar())
                {
                    worldTexture.Apply();
                    rawImage.texture = worldTexture;
                    yield return null;
                }
            }
        }

        worldTexture.Apply();
        rawImage.texture = worldTexture;

        Debug.Log(Time.realtimeSinceStartup);
        yield return null;
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
