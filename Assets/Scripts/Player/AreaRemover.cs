using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AreaRemover : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] private WorldTile hollowTile;
    [SerializeField] private CircleCollider2D removeArea;
    [SerializeField] private ParticleSystem digParticle;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 directionToMouse = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButton(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;
            Bounds colliderBounds = removeArea.bounds;

            Vector3Int min = tilemap.WorldToCell(colliderBounds.min);
            Vector3Int max = tilemap.WorldToCell(colliderBounds.max);

            Vector3 cirlceCenter = removeArea.transform.position;
            float radius = 1f;

            for (int worldY = min.y; worldY <= max.y; worldY++)
            {
                for (int worldX = min.x; worldX <= max.x; worldX++)
                {
                    Vector3Int tilePos = new Vector3Int(worldX, worldY, 0);
                    Vector3 worldTilePos = tilemap.GetCellCenterWorld(tilePos);

                    float distanceToCenter = Vector3.Distance(worldTilePos, cirlceCenter);
                    if (distanceToCenter <= radius)
                    {
                        if (tilemap.HasTile(tilePos))
                        {
                            WorldTile currentTile = tilemap.GetTile(tilePos) as WorldTile;

                            if (currentTile.ColliderType != Tile.ColliderType.None)
                            {
                                //Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, -directionToMouse);
                                //ParticleSystem ps = Instantiate(digParticle, transform.position, spawnRotation);
                                //ps.GetComponent<ParticleSystemRenderer>().material.color = currentTile.Color;

                                tilemap.SetTile(tilePos, hollowTile);
                                tilemap.SetColor(tilePos, hollowTile.Color);
                            }
                        }
                    }
                }

            }
        }
    }
}
