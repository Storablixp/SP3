using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Border Mutator", menuName = "Scriptable Objects/World Mutator/Border")]
public class BorderMutator : WorldMutatorSO
{
    [Header("Pixels")]
    public PixelSO BorderPixel;
    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                if (arrayX == 0 ||arrayX == worldSize.x - 1 || arrayY == endY)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, BorderPixel);
                }
            }
        }

        yield return null;
    }
}
