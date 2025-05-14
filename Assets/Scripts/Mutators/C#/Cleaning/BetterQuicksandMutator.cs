using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Voronoi", menuName = "Scriptable Objects/World Mutator/Biome Data/Voronoi")]
public class BetterQuicksandMutator : WorldMutatorSO
{
    [SerializeField] private int gridSize = 16;
    private int cellWidth;
    private int cellHeight;
    private Vector2Int[,] pointsPositions;

    public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        base.SetUp(worldGenerator, worldSize);

        cellWidth = worldSize.x / gridSize;
        cellHeight = worldSize.y / gridSize;

        GeneratePoints();
    }

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                int gridY = arrayY / cellHeight;
                int gridX = arrayX / cellWidth;

                float nearestDistance = Mathf.Infinity;
                Vector2Int nearestPoint = new Vector2Int();
                int nearestGridY = 0;
                int nearestGridX = 0;

                for (int a = -1; a <= 1; a++)
                {
                    for (int b = -1; b <= 1; b++)
                    {
                        int cellY = gridY + a;
                        int cellX = gridX + b;

                        if (cellX < 0 || cellY < 0 || cellX >= gridSize || cellY >= gridSize)
                            continue;

                        Vector2Int currentPoint = pointsPositions[cellX, cellY];
                        float distance = Vector2Int.Distance(new Vector2Int(arrayX, arrayY), currentPoint);

                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestPoint = currentPoint;
                            nearestGridX = cellX;
                            nearestGridY = cellY;
                        }
                    }
                }

                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                //if (nearestGridY == startY)
                //{
                //    pixelInstance.Voronoi = 2;
                //}
                //else
                //{
                //    pixelInstance.Voronoi = -2;
                //}


                pixels[arrayX, arrayY] = pixelInstance;
            }
        }
        yield return null;
    }

    private void GeneratePoints()
    {
        pointsPositions = new Vector2Int[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                pointsPositions[i, j] = new Vector2Int(i * cellWidth + Random.Range(0, cellWidth), j * cellHeight + Random.Range(0, cellHeight));
            }
        }
    }
}

