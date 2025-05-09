using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Surface Mutator", menuName = "Scriptable Objects/World Mutator/Surface")]
public class SurfaceMutator : WorldMutatorSO
{
    [Header("Threshold")]
    [SerializeField, Range(0.0f, 1.0f)] private float sandThreshold;
    [SerializeField, Range(0.0f, 1.0f)] private float grassThreshold;

    [Header("Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO dirtPixel;
    [SerializeField] private PixelSO grassPixel;
    [SerializeField] private PixelSO sandPixel;
    [SerializeField] private PixelSO clayPixel;
    [SerializeField] private PixelSO placeholder;
    
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
                    if (pixels[arrayX, arrayY].Temperature > sandThreshold)
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, sandPixel);
                    }
                    else if (pixels[arrayX, arrayY].Temperature > grassThreshold)
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, grassPixel);
                    }
                    else
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY, clayPixel);
                    }
                }
            }
        }

        yield return null;
    }
}
