using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Fluid Mutator", menuName = "Scriptable Objects/World Mutator/Cave System/Fluid")]
public class FluidMutator : WorldMutatorSO
{
    [Header("Other Pixels")]
    [SerializeField] private PixelSO AirPixel;
    [SerializeField] private PixelSO HollowPixel;
    [SerializeField] private PixelSO DeepStonePixel;

    [Header("Terrain Pixels")]
    [SerializeField] private PixelSO DirtPixel;
    [SerializeField] private PixelSO SandPixel;
    [SerializeField] private PixelSO SnowPixel;
    [SerializeField] private PixelSO VolcanicPixel;


    [Header("Fluid Pixels")]
    [SerializeField] private PixelSO IcePixel;
    [SerializeField] private PixelSO ClayPixel;
    [SerializeField] private PixelSO LavaPixel;
    [SerializeField] private PixelSO QuicksandPixel;
    [SerializeField] private PixelSO WaterPixel;


    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];
                if (pixelInstance.Pixel == AirPixel || pixelInstance.Pixel == HollowPixel) continue;

                if (pixelInstance.Wetness >= 1)
                {
                    AddWetPixels(arrayX, arrayY, pixelInstance);
                }
                else
                {
                    AddDryPixels(arrayX, arrayY, pixelInstance);
                }
            }
        }

        MoveIce(worldSize, pixels);
        yield return null;
    }

    private void AddWetPixels(int arrayX, int arrayY, PixelInstance pixelInstance)
    {
        if (pixelInstance.Depth <= 1 && pixelInstance.Depth > -2)
        {
            if (pixelInstance.Temperature >= 2 && pixelInstance.Depth <= 0)
            {
                if (pixelInstance.Wetness >= 2)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, LavaPixel);
                }
            }
            if (pixelInstance.Temperature == 1)
            {
                if (pixelInstance.Wetness >= 2 && pixelInstance.Depth == 1)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, ClayPixel);
                }
            }
            if (pixelInstance.Temperature == -2)
            {
                worldGenerator.ChangePixel(arrayX, arrayY, IcePixel);
            }

            if (pixelInstance.Temperature == 0)
            {
                if (pixelInstance.Depth == 1)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, WaterPixel);
                }
                else if(pixelInstance.Depth <= 1 && pixelInstance.Wetness >= 2)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, WaterPixel);

                }
            }
        }

        else
        {
            worldGenerator.ChangePixel(arrayX, arrayY, LavaPixel);
        }
    }
    private void AddDryPixels(int arrayX, int arrayY, PixelInstance pixelInstance)
    {
        if (pixelInstance.Wetness == -2)
        {
            if (pixelInstance.Temperature >= 0 && pixelInstance.Temperature < 2 && pixelInstance.Depth > -1)
            {
                worldGenerator.ChangePixel(arrayX, arrayY, SandPixel);
            }
        }
    }
    private void MoveIce(Vector2Int worldSize, PixelInstance[,] pixels)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];

                if (pixel.Pixel != IcePixel ||
               GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, AirPixel)) continue;

                if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, SnowPixel))
                {
                    int y = 0;
                    do
                    {
                        worldGenerator.ChangePixel(arrayX, arrayY + 1 + y, IcePixel);
                        worldGenerator.ChangePixel(arrayX, arrayY + y, SnowPixel);
                        y++;
                    }


                    while (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY + y, Vector2Int.up, worldGenerator, SnowPixel));
                }
                else
                {
                    if (arrayY + 1 < startY)
                    {
                        PixelSO pixelBeneath = pixels[arrayX, arrayY + 1].Pixel;
                        worldGenerator.ChangePixel(arrayX, arrayY, pixelBeneath);
                    }
                }
            }
        }
    }

}
