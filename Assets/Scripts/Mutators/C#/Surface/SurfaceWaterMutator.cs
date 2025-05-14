using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Surface Water", menuName = "Scriptable Objects/World Mutator/Surface/Surface Water")]
public class SurfaceWaterMutator : WorldMutatorSO
{
    [Header("Warm Pixels")]
    [SerializeField] private PixelSO SandPixel;
    [SerializeField] private PixelSO QuicksandPixel;

    [Header("Neutral Pixels")]
    [SerializeField] private PixelSO DirtPixel;
    [SerializeField] private PixelSO WaterPixel;

    [Header("Cold Pixels")]
    [SerializeField] private PixelSO SnowPixel;
    [SerializeField] private PixelSO IcePixel;

    [Header("Other Pixels")]
    [SerializeField] private PixelSO AirPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();

        PlaceWater(worldSize, pixels);
        MoveUp(worldSize, pixels);
        yield return null;
    }

    private void PlaceWater(Vector2Int worldSize, PixelInstance[,] pixels)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];
                if (pixelInstance.Pixel == AirPixel) continue;

                if (pixelInstance.Depth == 1)
                {
                    if (pixelInstance.Wetness >= 1)
                    {
                        if (pixelInstance.SunlightLevel >= 2)
                        {
                            if (pixelInstance.Wetness >= 2)
                            {
                                worldGenerator.ChangePixel(arrayX, arrayY, QuicksandPixel);
                            }
                        }
                        else if (pixelInstance.SunlightLevel <= -2)
                        {
                            worldGenerator.ChangePixel(arrayX, arrayY, IcePixel);
                        }
                        else worldGenerator.ChangePixel(arrayX, arrayY, WaterPixel);

                    }
                }
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
                pair.Add(new PixelPair
                {
                    liquidPixel = WaterPixel,
                    terrainPixel = DirtPixel
                });
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
                        PixelSO pixelBeneath = pixels[arrayX, arrayY + 1].Pixel;
                        worldGenerator.ChangePixel(arrayX, arrayY, pixelBeneath);
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
