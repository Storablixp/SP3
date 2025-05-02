using UnityEngine;

[CreateAssetMenu(fileName = "New PixelSO", menuName = "Scriptable Objects/PixelSO")]
public class PixelSO : ScriptableObject
{
    public bool Unchangeable;
    public Color Color = Color.white;
}
