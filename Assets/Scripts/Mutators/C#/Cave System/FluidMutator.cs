using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Fluid Mutator", menuName = "Scriptable Objects/World Mutator/Cave System/Fluid")]
public class FluidMutator : WorldMutatorSO
{
    [Header("Other Pixels")]
    [SerializeField] private PixelSO AirPixel;
    [SerializeField] private PixelSO HollowPixel;

    [Header("Terrain Pixels")]
    [SerializeField] private PixelSO DirtPixel;
    [SerializeField] private PixelSO SandPixel;
    [SerializeField] private PixelSO SnowPixel;


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
                if (pixelInstance.Pixel == AirPixel) continue;

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

        MoveUp(worldSize, pixels);
        yield return null;
    }

    private void AddWetPixels(int arrayX, int arrayY, PixelInstance pixelInstance)
    {
        if (pixelInstance.Depth <= 1 && pixelInstance.Depth > -2)
        {

            //if (pixelInstance.Temperature >= 2)
            //{
            //    if (pixelInstance.Wetness >= 2)
            //    {
            //        worldGenerator.ChangePixel(arrayX, arrayY, LavaPixel);
            //    }
            //}
            if (pixelInstance.Temperature == 1)
            {
                if (pixelInstance.Wetness >= 2)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, QuicksandPixel);
                }
            }
            else if (pixelInstance.Temperature <= -1)
            {
                worldGenerator.ChangePixel(arrayX, arrayY, IcePixel);
            }
            else if (pixelInstance.Temperature == 0) worldGenerator.ChangePixel(arrayX, arrayY, WaterPixel);

            if (pixelInstance.Temperature == 0)
            {
                if (pixelInstance.Wetness == 1 && pixelInstance.Depth == 1)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, ClayPixel);
                }
                else worldGenerator.ChangePixel(arrayX, arrayY, WaterPixel);
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
            if (pixelInstance.Temperature == 0)
            {
                worldGenerator.ChangePixel(arrayX, arrayY, SandPixel);
            }
        }
    }

    private void MoveUp(Vector2Int worldSize, PixelInstance[,] pixels)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixel = pixels[arrayX, arrayY];

                List<PixelPair> pair = new List<PixelPair>();
                pair.Add(new PixelPair
                {
                    liquidPixel = QuicksandPixel,
                    terrainPixel = SandPixel
                });
                //pair.Add(new PixelPair
                //{
                //    liquidPixel = WaterPixel,
                //    terrainPixel = DirtPixel
                //});
                pair.Add(new PixelPair
                {
                    liquidPixel = IcePixel,
                    terrainPixel = SnowPixel
                });

                for (int i = 0; i < pair.Count; i++)
                {

                    if (pixel.Pixel != pair[i].liquidPixel ||
                   GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, AirPixel)) continue;

                    if (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, pair[i].terrainPixel))
                    {
                        int y = 0;
                        do
                        {
                            worldGenerator.ChangePixel(arrayX, arrayY + 1 + y, pair[i].liquidPixel);
                            worldGenerator.ChangePixel(arrayX, arrayY + y, pair[i].terrainPixel);
                            y++;
                        }


                        while (GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY + y, Vector2Int.up, worldGenerator, pair[i].terrainPixel));
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

    private class PixelPair
    {
        public PixelSO liquidPixel;
        public PixelSO terrainPixel;
    }
}
