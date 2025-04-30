using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Cellular Automata Mutator", menuName = "Scriptable Objects/World Mutator/Cellular Automata")]
public class CellularAutomataMutator : WorldMutatorSO
{
    [Header("Tiles")]
    public PixelSO PixelToCount;
    public PixelSO PixelToReplace;

    [Header("Settings")]
    public int ReplacementThreshold = 4;
    public int MooreNeighborhoodSize = 1;
    public int Iterations = 5;

    public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        base.SetUp(worldGenerator, worldSize);
    }

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        //PixelSO[,] updatedTileData = new PixelSO[worldSize.x, worldSize.y];

        //for (int i = 0; i < Iterations; i++)
        //{
        //    if (visuals && i != 0) worldGenerator.ResetCounterValues();

        //    // First pass: calculate new states
        //    for (int arrayY = startY; arrayY >= endY; arrayY--)
        //    {
        //        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        //        {
        //            TileInstance tileInstance = chunk.Tiles[arrayX, arrayY];
        //            if (tileInstance == null) continue;
        //            if (tileInstance.WorldTile != PixelToReplace && tileInstance.WorldTile != PixelToCount) continue;


        //            int desiredNeighbors = 0;
        //            for (int dy = -MooreNeighborhoodSize; dy <= MooreNeighborhoodSize; dy++)
        //            {
        //                for (int dx = -MooreNeighborhoodSize; dx <= MooreNeighborhoodSize; dx++)
        //                {
        //                    if (dx == 0 && dy == 0) continue;

        //                    int nx = arrayX + dx;
        //                    int ny = arrayY + dy;

        //                    if (worldGenerator.IsInBounds(nx, ny))
        //                    {
        //                        TileInstance neighbor = chunk.Tiles[nx, ny];
        //                        if (neighbor != null && neighbor.WorldTile == PixelToCount)
        //                        {
        //                            desiredNeighbors++;
        //                        }
        //                    }
        //                }
        //            }

        //            if (desiredNeighbors > ReplacementThreshold)
        //            {
        //                updatedTileData[arrayX, arrayY] = PixelToCount;
        //            }
        //            else updatedTileData[arrayX, arrayY] = PixelToReplace;
        //        }
        //    }

        //    // Second pass: apply changes
        //    for (int arrayY = startY; arrayY >= endY; arrayY--)
        //    {
        //        for (int arrayX = 0; arrayX < chunkWidth; arrayX++)
        //        {
        //            TileInstance tileInstance = chunk.Tiles[arrayX, arrayY];
        //            if (tileInstance == null) continue;
        //            if (tileInstance.WorldTile != PixelToReplace && tileInstance.WorldTile != PixelToCount) continue;

        //            WorldTile newTile = updatedTileData[arrayX, arrayY];
        //            if (newTile == null) continue;

        //            tileInstance.WorldTile = newTile;
        //            tileInstance.Health = newTile.Health;

        //            worldGenerator.PlaceTile(arrayX, arrayY, chunk, newTile);

        //            if (visuals)
        //            {
        //                if (worldGenerator.UpdateProgressbar(false))
        //                {
        //                    yield return null;
        //                }
        //            }
        //        }
        //    }
        //    if (visuals) worldGenerator.UpdateProgressbar(true);
        //}

        //if (!visuals)
        //{
        //    for (int i = 0; i < Iterations; i++)
        //    {
        //        worldGenerator.UpdateProgressbar(true);
        //    }
        //}

        yield return null;
    }
}
