using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Remove Small Holes", menuName = "Scriptable Objects/World Mutator/Cleaning/Small Holes")]
public class RemoveSmallHoles : WorldMutatorSO
{
    [SerializeField] private PixelSO hollowPixel;
    [SerializeField] private PixelSO placeholderPixel;
    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                if (pixels[arrayX, arrayY].Pixel == hollowPixel &&
                    !GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 2, hollowPixel, 9))
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, placeholderPixel);
                    //for (int i = -3; i <= 3; i++)
                    //{
                    //    for (int j = -3; j <= 3; j++)
                    //    {
                    //        worldGenerator.ChangePixel(arrayX + i, arrayY + j, placeholderPixel);
                    //    }
                    //}
                }
            }
        }

        yield return null;
    }
}
