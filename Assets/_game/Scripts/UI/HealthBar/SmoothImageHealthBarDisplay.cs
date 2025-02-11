using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SmoothImageHealthBarDisplay : SmoothHealthBarBase
{
    [SerializeField] private Image _image;

    protected override void SetInstantValue(float value)
    {
        _image.fillAmount = value;
    }

    protected override void SetDisplayValue(float value)
    {
        _image.fillAmount = value;
    }
}