using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Biome Mutator", menuName = "Scriptable Objects/World Mutator/Content/Biome")]
public class BiomeMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO dirtPixel;
    [SerializeField] private PixelSO deepStonePixel;
    [SerializeField] private PixelSO hollowPixel;
    [SerializeField] private PixelSO lavaPixel;
    [SerializeField] private PixelSO sandPixel;
    [SerializeField] private PixelSO snowPixel;
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

                if (pixelInstance.Depth == 1 && pixelInstance.Pixel != hollowPixel)
                {
                    if (pixelInstance.Temperature == 2)
                    {
                        pixelToAdd = deepStonePixel;
                    }
                    else if (pixelInstance.Temperature == 1)
                    {
                        pixelToAdd = sandPixel;
                    }
                    else if (pixelInstance.Temperature == 0)
                    {
                        pixelToAdd = dirtPixel;
                    }
                    else if (pixelInstance.Temperature == -1)
                    {
                        pixelToAdd = snowPixel;
                    }
                    else
                    {
                        pixelToAdd = lavaPixel;
                    }
                    worldGenerator.ChangePixel(arrayX, arrayY, pixelToAdd);
                }
            }
        }

        yield return null;
    }
}
