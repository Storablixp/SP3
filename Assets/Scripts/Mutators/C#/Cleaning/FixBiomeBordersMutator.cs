using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Fix Biome Border", menuName = "Scriptable Objects/World Mutator/Content/Fix Biome Borders")]
public class FixBiomeBordersMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO dirtPixel;
    [SerializeField] private PixelSO sandPixel;
    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];

               if (pixelInstance.Pixel == sandPixel)
                {
                    if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.right, worldGenerator, dirtPixel))
                    {
                        int nr = Random.Range(0, 2);
                        if (nr == 1)
                        {
                            Debug.Log("C");
                            worldGenerator.ChangePixel(arrayX, arrayY, dirtPixel);
                        }
                    }
                }
            }
        }

        yield return null;
    }
}
