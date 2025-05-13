using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Water And Lava Mutator", menuName = "Scriptable Objects/World Mutator/Water And Lava")]
public class WaterLavaMutator : WorldMutatorSO
{
    [Header("Pixels")]
    [SerializeField] private PixelSO AirPixel;
    [SerializeField] private PixelSO HollowPixel;
    [SerializeField] private PixelSO IcePixel;
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

        yield return null;
    }
}
