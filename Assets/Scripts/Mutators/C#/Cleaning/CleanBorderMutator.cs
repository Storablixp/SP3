using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Clean Border Mutator", menuName = "Scriptable Objects/World Mutator/Cleaning/Clean Border")]
public class CleanBorderMutator : WorldMutatorSO
{
    [Header("Pixels")]
    public PixelSO AirPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                if (arrayX == 0)
                {
                    bool desiredNeighborExists = GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.right, worldGenerator, AirPixel);
                    if (desiredNeighborExists)
                    {
                        worldGenerator.TurnToAir(arrayX, arrayY);
                    }
                }

                if (arrayX == worldSize.x - 1)
                {
                    bool desiredNeighborExists = GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.left, worldGenerator, AirPixel);
                    if (desiredNeighborExists)
                    {
                        worldGenerator.TurnToAir(arrayX, arrayY);
                    }
                }
            }
        }

        yield return null;
    }
}
