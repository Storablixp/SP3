using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Terrain Mutator", menuName = "Scriptable Objects/World Mutator/Cave System/Terrain")]
public class TerrainMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO dirtPixel;
    [SerializeField] private PixelSO deepStonePixel;
    [SerializeField] private PixelSO stonePixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                PixelSO pixelToAdd;
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

                if (pixelInstance.Depth == 2)
                {
                    pixelToAdd = airPixel;
                }
                else if (pixelInstance.Depth == 1 && pixelInstance.Pixel)
                {
                    pixelToAdd = dirtPixel;
                }
                else if (pixelInstance.Depth == 0)
                {
                    pixelToAdd = stonePixel;
                }
                else
                {
                    pixelToAdd = deepStonePixel;
                }

                worldGenerator.ChangePixel(arrayX, arrayY, pixelToAdd);
            }
        }

        //bool foundDirt = false;
        //for (int arrayY = startY; arrayY >= endY; arrayY--)
        //{
        //    for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        //    {
        //        PixelInstance pixelInstance = pixels[arrayX, arrayY];

        //        if (pixelInstance.Depth != 1)
        //        {
        //            continue;
        //        }

        //        if (pixelInstance.Pixel == dirtPixel)
        //        {
        //            if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.right, worldGenerator, sandPixel))
        //            {
        //                foundDirt = true;
        //            }
        //        }

        //        if (pixelInstance.Pixel == sandPixel)
        //        {
        //            if (!foundDirt)
        //            {
        //                worldGenerator.ChangePixel(arrayX, arrayY, airPixel);
        //            }
        //        }
        //    }

        //    if (foundDirt)
        //    {
        //        break;
        //    }
        //}

        yield return null;
    }

}
