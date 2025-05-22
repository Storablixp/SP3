using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Surface Mutator", menuName = "Scriptable Objects/World Mutator/Surface")]
public class SurfaceMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO dirtPixel;
    [SerializeField] private PixelSO grassPixel;
    [SerializeField] private PixelSO hollowPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                if (pixels[arrayX, arrayY].Pixel == dirtPixel &&
                    GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, airPixel))
                {
                    if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.down, worldGenerator, dirtPixel))
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, grassPixel);
                    }
                    else
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, airPixel);
                    }
                }
                else if (pixels[arrayX, arrayY].Pixel == dirtPixel && arrayY == startY)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, grassPixel);
                }

                if(pixels[arrayX, arrayY].Pixel == hollowPixel &&
                    GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, 1, airPixel, 3))
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, airPixel);
                }

                if (worldGenerator.UpdateProgressbar(false))
                {
                    yield return null;
                }
            }
        }

        yield return null;
    }
}
