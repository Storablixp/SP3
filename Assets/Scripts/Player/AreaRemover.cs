using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AreaRemover : MonoBehaviour
{
    private WorldMap worldMap;
    [SerializeField] private WorldTile hollowTile;
    [SerializeField] private WorldTile airTile;
    [SerializeField] private CircleCollider2D removeArea;
    [SerializeField] private ParticleSystem digParticle;
    private Tilemap currentTilemap;
    private float diameter = 1f;
    
    private WorldGenerator worldGenerator;
    private Vector2Int worldSize;
    public void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize, WorldMap worldMap)
    {
        diameter = removeArea.radius * 2;
        this.worldGenerator = worldGenerator;
        this.worldSize = worldSize;
        this.worldMap = worldMap;
    }

    void Update()
    {
        //Rotation
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 directionToMouse = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButton(0))
        {
            if (currentTilemap == null) return;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;
            Bounds colliderBounds = removeArea.bounds;

            Vector3Int min = currentTilemap.WorldToCell(colliderBounds.min);
            Vector3Int max = currentTilemap.WorldToCell(colliderBounds.max);

            Vector3 cirlceCenter = removeArea.transform.position;

            for (int worldY = min.y; worldY <= max.y; worldY++)
            {
                for (int worldX = min.x; worldX <= max.x; worldX++)
                {
                    Vector3Int tilePos = new Vector3Int(worldX, worldY, 0);
                    Vector3 worldTilePos = currentTilemap.GetCellCenterWorld(tilePos);

                    float distanceToCenter = Vector3.Distance(worldTilePos, cirlceCenter);
                    if (distanceToCenter <= diameter)
                    {
                        if (currentTilemap.HasTile(tilePos))
                        {
                            int pixelX = tilePos.x + worldSize.x / 2;
                            int pixelY = tilePos.y + worldSize.y / 2;
                            Vector2Int lookupKey = new Vector2Int(pixelX, pixelY);
                            TileInstance tileInstance = worldGenerator.Tiles[lookupKey];


                            if (tileInstance.Tile.Unbreakable) continue;

                            if (tileInstance.Tile.ColliderType != Tile.ColliderType.None)
                            {
                                Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, -directionToMouse);
                                ParticleSystem ps = Instantiate(digParticle, transform.position, spawnRotation);
                                ps.GetComponent<ParticleSystemRenderer>().material.color = tileInstance.Tile.Color;

                                if (tileInstance.Tile.AirWhenRemoved)
                                {
                                    currentTilemap.SetTile(tilePos, airTile);
                                    currentTilemap.SetColor(tilePos, airTile.Color);
                                    worldGenerator.ReplaceTile(lookupKey, airTile);
                                }
                                else
                                {
                                    currentTilemap.SetTile(tilePos, hollowTile);
                                    currentTilemap.SetColor(tilePos, hollowTile.Color);
                                    worldGenerator.ReplaceTile(lookupKey, hollowTile);
                                }

                                worldMap.ChangePixel(lookupKey);
                            }
                        }
                    }
                }

            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (collision.gameObject.TryGetComponent(out Tilemap tilemap))
            {
                currentTilemap = tilemap;
            }
        }
    }
}
