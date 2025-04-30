using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Randomizer Mutator", menuName = "Scriptable Objects/World Mutator/Randomizer")]
public class RandomizerMutator : WorldMutatorSO
{
    [Header("Settings")]
    [Range(1, 100)] public int PercentageForA;

    [Header("Tiles")]
    public PixelSO PixelA;
    public PixelSO PixelB;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                int nr = Random.Range(1, 101);

                if (nr <= PercentageForA)
                {
                    worldGenerator.AddPixel(arrayX, arrayY, PixelA);
                }
                else
                {
                    worldGenerator.AddPixel(arrayX, arrayY, PixelB);
                }
            }
        }

        yield return null;
    }
}
