using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Surface Mutator", menuName = "Scriptable Objects/World Mutator/Surface")]
public class SurfaceMutator : WorldMutatorSO
{
    [Header("Threshold")]
    //[SerializeField, Range(0.0f, 1.0f)] private float grassThreshold;

    [Header("Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO dirtPixel;
    [SerializeField] private PixelSO grassPixel;
    [SerializeField] private PixelSO sandPixel;

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
                        worldGenerator.ChangePixel(arrayX, arrayY, grassPixel);
                        worldGenerator.ChangePixel(arrayX, arrayY - 1, dirtPixel);
                    }


                    //if (pixels[arrayX, arrayY].Temperature > grassThreshold)
                    //{
                    //    worldGenerator.ChangePixel(arrayX, arrayY, grassPixel);
                    //}

                }
            }
        }

        yield return null;
    }
}
