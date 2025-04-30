using UnityEngine;

public class GlobalEasingFunctions
{
    public enum EasingType { None, SmoothStep, SmoothStart, SmoothStop, AbsoluteValue };

    public static float GetEasingValue(float t, float n, EasingType type)
    {
        switch (type)
        {
            case EasingType.SmoothStep: return SmoothStepFunc(t);
            case EasingType.SmoothStart: return SmoothStartFunc(t, n);
            case EasingType.SmoothStop: return SmoothStopFunc(t, n);
            case EasingType.AbsoluteValue: return AbsoluteValueFunc(t, n);
            case EasingType.None:
            default:
                return t;
        }
    }
    private static float SmoothStepFunc(float t)
    {
        return t * t * (3f - 2f * t);
    }

    private static float SmoothStartFunc(float t, float n)
    {
        return Mathf.Pow(t, n);
    }

    private static float SmoothStopFunc(float t, float n)
    {
        return 1f - Mathf.Pow(1 - t, n);
    }

    private static float AbsoluteValueFunc(float t, float n)
    {
        return Mathf.Pow(Mathf.Abs(t - 0.5f) * 2, n);
    }
}
