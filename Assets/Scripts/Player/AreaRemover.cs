using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AreaRemover : MonoBehaviour
{
    [SerializeField] private WorldTile hollowTile;
    [SerializeField] private CircleCollider2D removeArea;
    [SerializeField] private ParticleSystem digParticle;
    private Tilemap currentTilemap;
    float diameter = 1f;

    private void Start()
    {
        diameter = removeArea.radius * 2;
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
                            WorldTile currentTile = currentTilemap.GetTile(tilePos) as WorldTile;

                            if (currentTile.Unbreakable) continue;

                            if (currentTile.ColliderType != Tile.ColliderType.None)
                            {
                                Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, -directionToMouse);
                                ParticleSystem ps = Instantiate(digParticle, transform.position, spawnRotation);
                                ps.GetComponent<ParticleSystemRenderer>().material.color = currentTile.Color;

                                currentTilemap.SetTile(tilePos, hollowTile);
                                currentTilemap.SetColor(tilePos, hollowTile.Color);
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
