using UnityEngine;

[CreateAssetMenu(fileName = "New Perlin2DSettings", menuName = "Scriptable Objects/Noise Settings/Perlin 2D")]
public class Perlin2DSettings : ScriptableObject
{
    [Header("Settings")]
    [Range(0.001f, 0.1f)] public float NoiseScale = 0.1f;
    [Range(1, 10)] public int Octaves = 3; //How many layers of nosie are stacked.
    public GlobalEasingFunctions.EasingType EasingFunctionType;
    [Range(0.01f, 3f)] public float EasingFunctionModifier = 3;

    [Header("Experimental")]
    [Range(0f, 1f)] public float Persistence = 0.5f; //A multiplier that determines how quickly the amplitudes diminish for each successive octave.
    [Range(1f, 2f)] public float Lacunarity = 2f;//A multiplier that determines how quickly the frequency increases for each successive octave.
}
