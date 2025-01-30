using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SmoothSliderHealthBarDisplay : SmoothHealthBarBase
{
    [SerializeField] private Slider _slider;

    protected override void SetInstantValue(float value)
    {
        _slider.value = value;
    }

    protected override void SetDisplayValue(float value)
    {
        _slider.value = value;
    }
}