using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Better Quicksand", menuName = "Scriptable Objects/World Mutator/Cleaning/Better Quicksand")]
public class BetterQuicksandMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO sandPixel;
    [SerializeField] private PixelSO quicksandPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];
                if (pixel.Pixel != quicksandPixel) continue;

                if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, sandPixel))
                {
                    worldGenerator.ChangePixel(arrayX, arrayY + 1, quicksandPixel);
                    worldGenerator.ChangePixel(arrayX, arrayY, sandPixel);
                }
            }
        }

        yield return null;
    }
}
