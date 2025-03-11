using System;
using TMPro;
using UnityEngine;

public class Singularity : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pullForce;

    [Serializable]
    private enum PullMode
    {
        Constant,
        LerpOverTime
    }

    [Header("Core Settings")]
    [SerializeField] private PullMode _pullMode = PullMode.Constant;
    [SerializeField] private ForceMode _forceMode = ForceMode.Acceleration;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private PullTarget[] _targets = null;

    [Header("Constant Mode")]
    [SerializeField] private float _gravityPull = 60;

    [Header("Lerp Mode")]
    [SerializeField] private float _startGravity = 50f;
    [SerializeField] private float _endGravity = 150f;
    [SerializeField] private float _lerpDuration = 3f;

    [Header("Light Settings")] //TODO: в отдельный компонент
    [SerializeField] private Light _pointLight;
    [SerializeField] private Light _directionalLight;
    [SerializeField] private Color _startColor = Color.green;
    [SerializeField] private Color _endColor = Color.red;

    private PullMode _previousPullMode;
    private float _radiusSqr;
    private float _currentGravity;
    private float _lerpTimer;
    private bool _isLerpDurationSafe;
    private bool _hasPointLight;
    private bool _hasDirectionalLight;


    private void Awake()
    {
        _radiusSqr = _radius * _radius;
        _currentGravity = _gravityPull;
        _previousPullMode = _pullMode;
        _isLerpDurationSafe = _lerpDuration > Mathf.Epsilon;
        _hasPointLight = _pointLight != null;
        _hasDirectionalLight = _directionalLight != null;

        ValidateTargets();
        InitializeLightColor();
    }

    private void InitializeLightColor()
    {
        if (_hasPointLight) _pointLight.color = _startColor;
        if (_hasDirectionalLight) _directionalLight.color = _startColor;
    }

    private void ValidateTargets()
    {
        if (_targets == null || _targets.Length == 0)
        {
            Debug.LogError("[Singularity] Targets array is not assigned", this);
            enabled = false;
            return;
        }

        for (int i = 0; i < _targets.Length; i++)
        {
            if (_targets[i] == null)
            {
                Debug.LogError("[Singularity] Missing reference in targets array", this);
                enabled = false;
                return;
            }
        }
    }

    private void Update()
    {
        _pullForce.text = $"Gravity force: {_currentGravity:F1}";
    }

    private void FixedUpdate()
    {
        UpdateGravityForce();
        ApplySingularityForces();
    }

    private void UpdateGravityForce()
    {
        if (_pullMode != _previousPullMode)
        {
            if (_pullMode == PullMode.LerpOverTime) _lerpTimer = 0f;
            _previousPullMode = _pullMode;
        }

        switch (_pullMode)
        {
            case PullMode.LerpOverTime:
                UpdateLerpMode();
                break;
            default:
                _currentGravity = _gravityPull;
                break;
        }
    }

    private void UpdateLerpMode()
    {
        if (!_isLerpDurationSafe) return;

        _lerpTimer += Time.fixedDeltaTime;
        float t = Mathf.PingPong(_lerpTimer / _lerpDuration, 1f);
        _currentGravity = Mathf.Lerp(_startGravity, _endGravity, t);

        UpdateLightColors(t);
    }

    private void UpdateLightColors(float t)
    {
        Color newColor = Color.Lerp(
            _startColor,
            _endColor,
            Mathf.SmoothStep(0f, 1f, t)
        );

        if (_hasPointLight) _pointLight.color = newColor;
        if (_hasDirectionalLight) _directionalLight.color = newColor;
    }

    //private void UpdateLightColor(float t)
    //{
    //    if (!_hasPointLight) return;

    //    _pointLight.color = Color.Lerp(
    //        _startColor,
    //        _endColor,
    //        Mathf.SmoothStep(0f, 1f, t)
    //    );
    //}

    private void ApplySingularityForces()
    {
        Vector3 centerPosition = transform.position;
        int targetsCount = _targets.Length;

        for (int i = 0; i < targetsCount; i++)
        {
            PullTarget pullTarget = _targets[i];
            if (pullTarget == null || !pullTarget.Pullable) continue;

            Rigidbody targetRb = pullTarget.TargetRigidbody;
            Vector3 targetPosition = targetRb.position;
            Vector3 toCenter = centerPosition - targetPosition;

            float distanceSqr = toCenter.sqrMagnitude;
            if (distanceSqr > _radiusSqr || distanceSqr < Mathf.Epsilon) continue;

            float distance = Mathf.Sqrt(distanceSqr);
            float gravityIntensity = distance / _radius;
            float massFactor = pullTarget.CachedMass * Time.fixedDeltaTime;

            targetRb.AddForce(
                toCenter * _currentGravity * gravityIntensity * massFactor,
                _forceMode
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _isLerpDurationSafe = _lerpDuration > Mathf.Epsilon;
        _hasPointLight = _pointLight != null;
        _hasDirectionalLight = _directionalLight != null;
    }
#endif
}







//public class Singularity : MonoBehaviour
//{
//    [SerializeField] private TextMeshProUGUI _pullForce;

//    [Serializable]
//    private enum PullMode
//    {
//        Constant,
//        LerpOverTime
//    }

//    [Header("Core Settings")]
//    [SerializeField] private PullMode _pullMode = PullMode.Constant;
//    [SerializeField] private ForceMode _forceMode = ForceMode.Acceleration;
//    [SerializeField] private float _radius = 5f;
//    [SerializeField] private PullTarget[] _targets = null;

//    [Header("Constant Mode")]
//    [SerializeField] private float _gravityPull = 60;

//    [Header("Lerp Mode")]
//    [SerializeField] private float _startGravity = 50f;
//    [SerializeField] private float _endGravity = 150f;
//    [SerializeField] private float _lerpDuration = 3f;

//    private float _radiusSqr;
//    private float _currentGravity;
//    private float _lerpTimer;
//    private PullMode _previousPullMode;
//    private bool _isLerpDurationSafe;

//    private void Awake()
//    {
//        _radiusSqr = _radius * _radius;
//        _currentGravity = _gravityPull;
//        _previousPullMode = _pullMode;
//        _isLerpDurationSafe = _lerpDuration > Mathf.Epsilon;

//        ValidateTargets();
//    }

//    private void ValidateTargets()
//    {
//        if (_targets == null || _targets.Length == 0)
//        {
//            Debug.LogError("[Singularity] Targets array is not assigned", this);
//            enabled = false;
//            return;
//        }

//        for (int i = 0; i < _targets.Length; i++)
//        {
//            if (_targets[i] == null)
//            {
//                Debug.LogError("[Singularity] Missing reference in targets array", this);
//                enabled = false;
//                return;
//            }
//        }
//    }

//    private void Update()
//    {
//        _pullForce.text = $"Gravity force: {_currentGravity}";
//    }

//    private void FixedUpdate()
//    {
//        UpdateGravityForce();
//        ApplySingularityForces();
//    }

//    private void UpdateGravityForce()
//    {
//        if (_pullMode != _previousPullMode)
//        {
//            if (_pullMode == PullMode.LerpOverTime) _lerpTimer = 0f;
//            _previousPullMode = _pullMode;
//        }

//        switch (_pullMode)
//        {
//            case PullMode.LerpOverTime:
//                UpdateLerpMode();
//                break;
//            default:
//                _currentGravity = _gravityPull;
//                break;
//        }
//    }

//    private void UpdateLerpMode()
//    {
//        if (!_isLerpDurationSafe) return;

//        _lerpTimer += Time.fixedDeltaTime;
//        float t = Mathf.PingPong(_lerpTimer / _lerpDuration, 1f);
//        _currentGravity = Mathf.Lerp(_startGravity, _endGravity, t);
//    }

//    private void ApplySingularityForces()
//    {
//        Vector3 centerPosition = transform.position;

//        for (int i = 0; i < _targets.Length; i++)
//        {
//            PullTarget pullTarget = _targets[i];
//            if (pullTarget == null || !pullTarget.Pullable) continue;

//            Rigidbody targetRb = pullTarget.TargetRigidbody;
//            Vector3 targetPosition = targetRb.position;
//            Vector3 toCenter = centerPosition - targetPosition;

//            float distanceSqr = toCenter.sqrMagnitude;
//            if (distanceSqr > _radiusSqr || distanceSqr < Mathf.Epsilon) continue;

//            float distance = Mathf.Sqrt(distanceSqr);
//            float gravityIntensity = distance / _radius;
//            float massFactor = pullTarget.CachedMass * Time.fixedDeltaTime;

//            targetRb.AddForce(
//                toCenter * _currentGravity * gravityIntensity * massFactor,
//                _forceMode
//            );
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(transform.position, _radius);
//    }

//#if UNITY_EDITOR
//    private void OnValidate()
//    {
//        _isLerpDurationSafe = _lerpDuration > Mathf.Epsilon;
//    }
//#endif
//}





//public class Singularity : MonoBehaviour
//{
//    [SerializeField] private float _gravityPull = 100f;
//    [SerializeField] private float _radius = 5f;
//    [SerializeField] private PullTarget[] _targets = null;
//    [SerializeField] private ForceMode _forceMode;

//    private float _radiusSqr;

//    private void Awake()
//    {
//        _radiusSqr = _radius * _radius;

//        for (int i = 0; i < _targets.Length; i++)
//        {
//            if (_targets[i] == null)
//            {
//                Debug.Log("[Singularity] target/s not assign");
//                enabled = false;
//                return;
//            }
//        }
//    }

//    private void FixedUpdate()
//    {
//        Vector3 centerPosition = transform.position;

//        for (int i = 0; i < _targets.Length; i++)
//        {
//            PullTarget pullTarget = _targets[i];

//            if (pullTarget == null || !pullTarget.Pullable)
//            {
//                Debug.Log("pullTarget == null ");
//                continue;
//            }

//            Vector3 targetPosition = pullTarget.TargetTransform.position;
//            Vector3 toCenter = centerPosition - targetPosition;
//            float distanceSqr = toCenter.sqrMagnitude;

//            if (distanceSqr > _radiusSqr)
//            {
//                Debug.Log("// Объект за пределами притяжения");

//                continue;
//            }

//            float distance = Mathf.Sqrt(distanceSqr);

//            if (distance < Mathf.Epsilon)
//            {
//                distance = Mathf.Epsilon;
//            }

//            float gravityIntensity = distance / _radius;

//            pullTarget.TargetRigidbody.AddForce(
//                toCenter * gravityIntensity * pullTarget.CachedMass * _gravityPull * Time.fixedDeltaTime,
//                _forceMode
//            );
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(transform.position, _radius);
//    }
//}