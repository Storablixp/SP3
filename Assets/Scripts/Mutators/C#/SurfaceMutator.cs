using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Surface Mutator", menuName = "Scriptable Objects/World Mutator/Surface")]
public class SurfaceMutator : WorldMutatorSO
{
    [Header("Required Mutators")]
    [SerializeField] private TemperatureMutator temperatureMutator;
    [SerializeField] private HumidityMutator humidityMutator;

    [Header("Pixels")]
    [SerializeField] private PixelSO airPixel;
    [SerializeField] private PixelSO dirtPixel;
    [SerializeField] private PixelSO grassPixel;
    [SerializeField] private PixelSO sandPixel;
    [SerializeField] private PixelSO clayPixel;
    [SerializeField] private PixelSO placeholder;
    
    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelSO[,] pixels = worldGenerator.RetrievePixels();

        for (int arrayY = startY; arrayY >= endY; arrayY--)
        {
            for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
            {
                if (pixels[arrayX, arrayY] == dirtPixel && 
                    GlobalNeighborCheckFucntions.SimpleCheck(arrayX, arrayY, Vector2Int.up, worldGenerator, airPixel))
                {
                    if (temperatureMutator.Temperatures[arrayX, arrayY] > 0.5f)
                    {
                        if (humidityMutator.Humidities[arrayX, arrayY] > 0.75f) worldGenerator.ChangePixel(arrayX, arrayY, sandPixel);
                        else worldGenerator.ChangePixel(arrayX, arrayY, grassPixel);
                    }
                    else
                    {
                        if (humidityMutator.Humidities[arrayX, arrayY] > 0.75f) worldGenerator.ChangePixel(arrayX, arrayY, clayPixel);
                        else worldGenerator.ChangePixel(arrayX, arrayY, grassPixel);
                    }
                }
            }
        }

        yield return null;
    }
}
