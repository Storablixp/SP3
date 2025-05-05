using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    private bool hasBeenSetUp;
    private Transform playerTrans;
    private ChunkInstance currentChunk;
    private Dictionary<Vector2Int, ChunkInstance> chunks;

    public void SetUp(Transform playerTrans, ChunkInstance currentChunk, Dictionary<Vector2Int, ChunkInstance> chunks)
    {
        this.playerTrans = playerTrans;
        this.currentChunk = currentChunk;
        this.chunks = chunks;

        UpdateCurrentChunk(currentChunk);
        hasBeenSetUp = true;
    }

    private void Update()
    {
        if (hasBeenSetUp)
        {
            Vector3 playerWorldPos = playerTrans.position;
            Vector3Int playerCellPos = currentChunk.Tilemap.WorldToCell(playerWorldPos);

            if (!currentChunk.Bounds.Contains(playerCellPos))
            {
                var newClosetChunk = FindClosestChunk();
                UpdateCurrentChunk(newClosetChunk);
            }

        }
    }

    private void UpdateCurrentChunk(ChunkInstance closestChunk)
    {
        HashSet<ChunkInstance> newActiveChunks = new HashSet<ChunkInstance>();
        ChunkInstance previousCurrent = currentChunk;

        // Step 1: Define which chunks should be active
        newActiveChunks.Add(closestChunk);
        foreach (var neighbor in closestChunk.NeighborChunks)
        {
            if (neighbor != null)
                newActiveChunks.Add(neighbor);
        }

        // Step 2: Deactivate old current and its neighbors if they're no longer needed
        if (previousCurrent != null)
        {
            if (!newActiveChunks.Contains(previousCurrent))
                previousCurrent.gameObject.SetActive(false);

            foreach (var neighbor in previousCurrent.NeighborChunks)
            {
                if (neighbor != null && !newActiveChunks.Contains(neighbor))
                    neighbor.gameObject.SetActive(false);
            }
        }

        // Step 3: Activate new current and its neighbors
        currentChunk = closestChunk;
        foreach (var chunk in newActiveChunks)
        {
            chunk.gameObject.SetActive(true);
        }
    }

    private ChunkInstance FindClosestChunk()
    {
        int playerX = Mathf.FloorToInt(playerTrans.position.x);
        int playerY = Mathf.FloorToInt(playerTrans.position.y);
        float shortestDistance = float.MaxValue;
        ChunkInstance closestChunk = null;

        foreach (var chunk in chunks)
        {
            float distance = Vector2Int.Distance(chunk.Key, new Vector2Int(playerX, playerY));
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestChunk = chunk.Value;
            }

        }

        return closestChunk;
    }

}
