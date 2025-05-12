using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Water And Lava Mutator", menuName = "Scriptable Objects/World Mutator/Water And Lava")]
public class WaterAndLavaMutator : WorldMutatorSO
{
    [Header("Settings")]
    [SerializeField, Range(0.0f, 1.0f)] private float waterThreshold;

    [Header("Pixels")]
    [SerializeField] private PixelSO WaterPixel;
    [SerializeField] private PixelSO HollowPixel;
    [SerializeField] private PixelSO AirPixel;

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();
        
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];
                if (pixelInstance.Pixel == AirPixel || pixelInstance.Pixel == HollowPixel) continue;

                if (pixels[arrayX, arrayY].Wetness > waterThreshold)
                {
                    worldGenerator.ChangePixel(arrayX, arrayY, WaterPixel);
                }

            }
        }

        yield return null;
    }
}
