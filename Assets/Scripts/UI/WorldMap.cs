using UnityEngine;
using UnityEngine.UI;

public class WorldMap : MonoBehaviour
{
    private RawImage rawImage;
    private Transform playerTrans;
    private RectTransform rectTransform;
    [SerializeField] private WorldGenerator worldGenerator;
    [SerializeField] private RectTransform playerMarker;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetUp(Vector2 worldSize, Texture texture)
    {
        rectTransform.sizeDelta = worldSize / 2;
        rawImage.texture = texture;
    }

    public void SetPlayer(Transform playerTrans) => this.playerTrans = playerTrans;

    private void Update()
    {
        if (rawImage != null && playerTrans != null)
        {
            playerMarker.anchoredPosition = playerTrans.position;
        }
    }

    public void ChangePixel(Vector2Int position)
    {
        Color color = worldGenerator.Tiles[position].Tile.Color;
        Texture2D texture = (Texture2D)rawImage.texture;
        texture.SetPixel(position.x, position.y, color);
        texture.Apply();
        rawImage.texture = texture;
    }
}
