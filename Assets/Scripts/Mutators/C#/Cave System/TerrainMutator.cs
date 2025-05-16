using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Terrain Mutator", menuName = "Scriptable Objects/World Mutator/Cave System/Terrain")]
public class TerrainMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO dirtPixel;
    [SerializeField] private PixelSO deepStonePixel;
    [SerializeField] private PixelSO hollowPixel;
    [SerializeField] private PixelSO icePixel;
    [SerializeField] private PixelSO volcanicRock;
    [SerializeField] private PixelSO sandPixel;
    [SerializeField] private PixelSO scorchedRockPixel;
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

                if (pixelInstance.Depth == 2)
                {
                    pixelToAdd = airPixel;
                }
                else if (pixelInstance.Depth == 1 && pixelInstance.Pixel)
                {
                    pixelToAdd = DetermineSurfacePixels(arrayX, arrayY, pixelInstance);
                }
                else if (pixelInstance.Depth == 0)
                {
                    //pixelToAdd = stonePixel;
                    pixelToAdd = DetermineCavePixels(arrayX, arrayY, pixelInstance);
                }
                else
                {
                    pixelToAdd = deepStonePixel;
                }

                worldGenerator.ChangePixel(arrayX, arrayY, pixelToAdd);
            }
        }
        yield return null;
    }

    private PixelSO DetermineSurfacePixels(int arrayX, int arrayY, PixelInstance pixelInstance)
    {
        if (pixelInstance.Temperature == 2)
        {
            return volcanicRock;
        }
        else if (pixelInstance.Temperature == 1)
        {
            return sandPixel;
        }
        else if (pixelInstance.Temperature == 0)
        {
            return dirtPixel;
        }
        else if (pixelInstance.Temperature == -1)
        {
            return snowPixel;
        }
        else
        {
            return icePixel;
        }
    }

    private PixelSO DetermineCavePixels(int arrayX, int arrayY, PixelInstance pixelInstance)
    {
        if (pixelInstance.Temperature == 2)
        {
            return volcanicRock;
        }
        else if (pixelInstance.Temperature == 1)
        {
            return stonePixel;
        }
        else if (pixelInstance.Temperature == 0)
        {
            return stonePixel;
        }
        else if (pixelInstance.Temperature == -1)
        {
            return snowPixel;
        }
        else
        {
            return icePixel;
        }
    }
}
