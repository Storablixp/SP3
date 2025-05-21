using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Water Neighbor Mutator", menuName = "Scriptable Objects/World Mutator/Cleaning/Water Neighbor")]
public class WaterNeighborMutator : WorldMutatorSO
{
    [Header("Settings")]
    public int moorseSize = 1;
    public uint threshold = 1;


    [Header("Pixels")]
    [SerializeField] private PixelSO waterPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                if (pixelInstance.Pixel == waterPixel)
                {
                    if (!GlobalNeighborCheckFucntions.MooreCheck(arrayX, arrayY, worldGenerator, moorseSize, waterPixel, threshold))
                    {
                        if(worldGenerator.IsInBounds(arrayX, arrayY - 1))
                        {
                            worldGenerator.ChangePixel(arrayX, arrayY, pixels[arrayX, arrayY -1].Pixel);
                        }
                    }
                }
            }
        }

        yield return null;
    }
}
