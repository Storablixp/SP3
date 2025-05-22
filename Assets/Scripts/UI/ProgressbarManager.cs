using UnityEngine;
using UnityEngine.UI;

public class ProgressbarManager : MonoBehaviour
{
    [SerializeField] private Slider functionSlider;
    [SerializeField] private Slider totalSlider;

    private void Start()
    {
        functionSlider.value = functionSlider.minValue;
        totalSlider.value = totalSlider.minValue;
    }

    public void ResetSlider()
    {
        functionSlider.value = 0;
    }
    public void UpdateFunctionSlider(float value)
    {
        functionSlider.value = value;
    }

    public void UpdateTotalSlider()
    {
        totalSlider.value++;
    }

    public void IncreaseTotalSliderMaxValue(int value) => totalSlider.maxValue += value;
}
