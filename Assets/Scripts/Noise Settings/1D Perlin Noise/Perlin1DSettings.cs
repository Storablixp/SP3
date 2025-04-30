using UnityEngine;

[CreateAssetMenu(fileName = "New Perlin1DSettings", menuName = "Scriptable Objects/Noise Settings/Perlin 1D")]
public class Perlin1DSettings : ScriptableObject
{
    [Range(0.01f, 0.1f)] public float NoiseScale = 0.1f;
    [Range(1, 10)] public int Octaves = 3; //How many layers of nosie are stacked.
    [Range(0.5f, 5f)] public float Frequency = 0.1f; //Low == Smooth. High == Tight.
    public float Persistence = 0.5f; //A multiplier that determines how quickly the amplitudes diminish for each successive octave.
    public float Lacunarity = 2f;//A multiplier that determines how quickly the frequency increases for each successive octave.
    [Range(0.01f, 0.99f)] public float noiseRangeMin = 0.75f;

    public GlobalEasingFunctions.EasingType EasingFunctionType;
    public float EasingFunctionModifier = 3;
}
