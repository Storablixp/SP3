using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Temperature Mutator", menuName = "Scriptable Objects/World Mutator/Pixel Data/Temperature")]
public class TemperatureMutator : WorldMutatorSO
{
    [SerializeField] private List<TemperatureArea> temperatureAreas = new();
    private Dictionary<int, TemperatureArea> temperatureAreaLookup = new();
    private int currentBiomeIndex;
    private int currentSize = 0;
    private int desiredSize;

    private int direction;

    public override void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        base.SetUp(worldGenerator, worldSize);
        currentSize = 0;
        temperatureAreaLookup.Clear();

        int nr = -2;
        for (int i = 0; i < temperatureAreas.Count; i++)
        {
            temperatureAreaLookup.Add(nr, temperatureAreas[i]);
            nr++;
        }

        //currentBiomeIndex = -2;
        //direction = 1;
        currentBiomeIndex = Random.Range(-2, 3);
        desiredSize = Random.Range(temperatureAreaLookup[currentBiomeIndex].MinLength, temperatureAreaLookup[currentBiomeIndex].MaxLength + 1);
    }

    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        PixelInstance[,] pixels = worldGenerator.RetrievePixels();
        AssignValuesToFirstRow(worldSize, pixels);
        AssignValuesToOtherRows(worldSize, pixels);
        yield return null;
    }

    private void AssignValuesToFirstRow(Vector2Int worldSize, PixelInstance[,] pixels)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            PixelInstance pixelInstance = pixels[arrayX, 0];

            if (currentSize >= desiredSize)
            {
                if (direction == 1)
                {
                    if (currentBiomeIndex + 1 > 2)
                    {
                        direction = -1;
                        currentBiomeIndex = 1;
                    }
                    else currentBiomeIndex++;
                }
                else
                {
                    if (currentBiomeIndex - 1 < -2)
                    {
                        direction = 1;
                        currentBiomeIndex = -1;
                    }
                    else currentBiomeIndex--;
                }

                //int nr = Random.Range(0, 2);

                //if (nr == 0)
                //{
                //    if (currentBiomeIndex - 1 < -2)
                //    {
                //        currentBiomeIndex = -1;
                //    }
                //    else currentBiomeIndex--;
                //}
                //else
                //{
                //    if (currentBiomeIndex + 1 > 2)
                //    {
                //        currentBiomeIndex = 1;
                //    }
                //    else currentBiomeIndex++;
                //}

                currentSize = 0;
                desiredSize = Random.Range(temperatureAreaLookup[currentBiomeIndex].MinLength, temperatureAreaLookup[currentBiomeIndex].MaxLength + 1);
            }

            pixelInstance.Temperature = currentBiomeIndex;
            pixels[arrayX, 0] = pixelInstance;
            currentSize++;
        }
    }

    private void AssignValuesToOtherRows(Vector2Int worldSize, PixelInstance[,] pixels)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
                PixelInstance pixelInstance = pixels[arrayX, arrayY];
                pixelInstance.Temperature = pixels[arrayX, 0].Temperature;
                pixels[arrayX, arrayY] = pixelInstance;

            }
        }
    }

    [System.Serializable]
    public struct TemperatureArea
    {
        public string Name;
        [Range(32, 256)] public int MinLength;
        [Range(32, 256)] public int MaxLength;
    }
}
