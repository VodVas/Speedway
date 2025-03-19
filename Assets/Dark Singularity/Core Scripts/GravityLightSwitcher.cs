using System;
using UnityEngine;

[Serializable]
public class GravityLightSwitcher : MonoBehaviour
{
    [SerializeField] private Light _pointLight;
    [SerializeField] private Light _directionalLight;
    [SerializeField] private Color _startColor = Color.green;
    [SerializeField] private Color _endColor = Color.red;

    private bool _hasPointLight;
    private bool _hasDirectionalLight;

    private void Awake()
    {
        _hasPointLight = _pointLight != null;
        _hasDirectionalLight = _directionalLight != null;
    }

    public void InitializeLightColor()
    {
        if (_hasPointLight) _pointLight.color = _startColor;
        if (_hasDirectionalLight) _directionalLight.color = _startColor;
    }

    public void UpdateColor(float t)
    {
        Color newColor = Color.Lerp(
            _startColor,
            _endColor,
            Mathf.SmoothStep(0f, 1f, t)
        );

        if (_hasPointLight) _pointLight.color = newColor;
        if (_hasDirectionalLight) _directionalLight.color = newColor;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _hasPointLight = _pointLight != null;
        _hasDirectionalLight = _directionalLight != null;
    }
#endif
}