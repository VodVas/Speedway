using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(GravityLightSwitcher))]
public class Singularity : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pullForce;

    [Serializable]
    private enum PullMode
    {
        Constant,
        LerpOverTime,
        LerpWithDelay
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

    [Header("Lerp With Delay Mode")]
    [SerializeField] private float _delayAtStart = 1f; //TODO: добавить шейдер дыма на машину // добавить ко всем ошибкам this (Debug.LogWarning("Explosion sound clip is not assigned!", this);)
    [SerializeField] private float _delayAtEnd = 1f;

    [Header("Light Settings")]
    [SerializeField] private bool _isLightToggleOn = false;

    private GravityLightSwitcher _lightController;
    private PullMode _previousPullMode;
    private float _radiusSqr;
    private float _currentGravity;
    private float _lerpTimer;
    private bool _isLerpDurationSafe;
    private bool _hasLightController;

    private enum LerpState
    {
        MovingToEnd,
        DelayAtEnd,
        MovingToStart,
        DelayAtStart
    }
    private LerpState _currentLerpState = LerpState.MovingToEnd;
    private float _currentDelayTimer = 0f;

    private void Awake()
    {
        _lightController = GetComponent<GravityLightSwitcher>();

        _radiusSqr = _radius * _radius;
        _currentGravity = _gravityPull;
        _previousPullMode = _pullMode;
        _isLerpDurationSafe = _lerpDuration > Mathf.Epsilon;
        _hasLightController = _lightController != null;

        ValidateTargets();

        if (_isLightToggleOn && _hasLightController)
            _lightController.InitializeLightColor();
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
            ResetLerpState();
            _previousPullMode = _pullMode;
        }

        switch (_pullMode)
        {
            case PullMode.LerpOverTime:
                UpdateLerpMode();
                break;
            case PullMode.LerpWithDelay:
                UpdateLerpWithDelayMode();
                break;
            default:
                _currentGravity = _gravityPull;
                break;
        }
    }

    private void ResetLerpState()
    {
        _lerpTimer = 0f;
        _currentDelayTimer = 0f;
        _currentLerpState = LerpState.MovingToEnd;
    }

    private void UpdateLerpMode()
    {
        if (!_isLerpDurationSafe) return;

        _lerpTimer += Time.fixedDeltaTime;
        float t = Mathf.PingPong(_lerpTimer / _lerpDuration, 1f);
        _currentGravity = Mathf.Lerp(_startGravity, _endGravity, t);

        if (_isLightToggleOn && _hasLightController)
            _lightController.UpdateColor(t);
    }

    private void UpdateLerpWithDelayMode()
    {
        if (!_isLerpDurationSafe) return;

        switch (_currentLerpState)
        {
            case LerpState.MovingToEnd:
                HandleMovingToEnd();
                break;
            case LerpState.DelayAtEnd:
                HandleDelayAtEnd();
                break;
            case LerpState.MovingToStart:
                HandleMovingToStart();
                break;
            case LerpState.DelayAtStart:
                HandleDelayAtStart();
                break;
        }

        UpdateLightForLerpWithDelay();
    }

    private void HandleMovingToEnd()
    {
        _lerpTimer += Time.fixedDeltaTime;
        if (_lerpTimer >= _lerpDuration)
        {
            _currentGravity = _endGravity;
            _currentLerpState = LerpState.DelayAtEnd;
            _currentDelayTimer = 0f;
        }
        else
        {
            float t = _lerpTimer / _lerpDuration;
            _currentGravity = Mathf.Lerp(_startGravity, _endGravity, t);
        }
    }

    private void HandleDelayAtEnd()
    {
        _currentDelayTimer += Time.fixedDeltaTime;
        if (_currentDelayTimer >= _delayAtEnd)
        {
            _currentLerpState = LerpState.MovingToStart;
            _lerpTimer = _lerpDuration;
        }
    }

    private void HandleMovingToStart()
    {
        _lerpTimer -= Time.fixedDeltaTime;
        if (_lerpTimer <= 0)
        {
            _currentGravity = _startGravity;
            _currentLerpState = LerpState.DelayAtStart;
            _currentDelayTimer = 0f;
        }
        else
        {
            float t = _lerpTimer / _lerpDuration;
            _currentGravity = Mathf.Lerp(_startGravity, _endGravity, t);
        }
    }

    private void HandleDelayAtStart()
    {
        _currentDelayTimer += Time.fixedDeltaTime;
        if (_currentDelayTimer >= _delayAtStart)
        {
            _currentLerpState = LerpState.MovingToEnd;
            _lerpTimer = 0f;
        }
    }

    private void UpdateLightForLerpWithDelay()
    {
        if (!_isLightToggleOn || !_hasLightController) return;

        float t = Mathf.InverseLerp(_startGravity, _endGravity, _currentGravity);
        _lightController.UpdateColor(t);
    }

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
        _hasLightController = _lightController != null;
    }
#endif
}