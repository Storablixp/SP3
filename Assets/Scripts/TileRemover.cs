using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRemover : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] private WorldTile hollowTile;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

            if (tilemap.HasTile(cellPosition))
            {
                WorldTile currentTile = (tilemap.GetTile(cellPosition)) as WorldTile;

                if (currentTile.ColliderType != Tile.ColliderType.None)
                {
                    tilemap.SetTile(cellPosition, hollowTile);
                    tilemap.SetColor(cellPosition, hollowTile.Color);
                }
            }
        }
    }
}
