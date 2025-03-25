using UnityEngine;

[ExecuteAlways]
public class WaterOptimizer : MonoBehaviour
{
    [SerializeField] private Material _waterMaterial;
    [SerializeField] private bool _autoDetectPlatform = true;
    [SerializeField] private bool _forceMobileOptimization;
    [Range(0.1f, 2f)] public float mobileWaveSpeed = 0.5f;

    private bool _isMobile;
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();

        if (_autoDetectPlatform)
        {
#if !UNITY_EDITOR
            _isMobile = YandexGame.EnvironmentData.isMobile;
#else
            _isMobile = _forceMobileOptimization;
#endif
        }
        else
        {
            _isMobile = _forceMobileOptimization;
        }

        ApplyPlatformOptimizations();
    }

    void ApplyPlatformOptimizations()
    {
        if (!_isMobile || _renderer == null || _waterMaterial == null)
        {
            enabled = false;
            return;
        }

        _renderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        _renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        _renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _renderer.receiveShadows = false;
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_WaveSpeed", mobileWaveSpeed);
        _renderer.SetPropertyBlock(_propBlock);

        _waterMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
    }

    void OnValidate()
    {
        if (_propBlock != null && _isMobile)
        {
            _propBlock.SetFloat("_WaveSpeed", mobileWaveSpeed);

            if (_renderer != null)
            {
                _renderer.SetPropertyBlock(_propBlock);
            }
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        if (_forceMobileOptimization && !Application.isPlaying)
        {
            ApplyPlatformOptimizations();
        }
    }
#endif
}