using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Reset World Mutator", menuName = "Scriptable Objects/World Mutator/Reset")]
public class ResetWorldMutator : WorldMutatorSO
{
    public PixelSO PlaceHolderPixel;
    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                worldGenerator.ChangePixel(arrayX, arrayY, PlaceHolderPixel);
            }
        }

        yield return null;
    }
}
