using System.Collections;
using UnityEngine;

public class BoxOpener : MonoBehaviour
{
    [SerializeField] private GameObject _boxLid;
    [SerializeField] private float _openDuration = 2f;
    [SerializeField] private float _targetXRotation = 150f;

    private Transform _lidTransform;
    private Quaternion _initialRotation;
    private Quaternion _targetRotation;
    private float _elapsedTime;
    private bool _isOpening;

    private void Awake()
    {
        if (_boxLid != null)
        {
            _lidTransform = _boxLid.transform;
        }
        else
        {
            Debug.LogError("BoxOpener: _boxLid not assign!", this);
            enabled = false;
            return;
        }
    }

    [ContextMenu("OpenBox")]
    public void OpenBox()
    {
        if (_lidTransform == null || _isOpening)
        {
            return;
        }

        _initialRotation = _lidTransform.localRotation;
        _targetRotation = Quaternion.Euler(_targetXRotation, _lidTransform.localRotation.eulerAngles.y, _lidTransform.localRotation.eulerAngles.z);
        _elapsedTime = 0f;
        _isOpening = true;

        StartCoroutine(OpenLidCoroutine());
    }

    private IEnumerator OpenLidCoroutine()
    {
        while (_elapsedTime < _openDuration)
        {
            _elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsedTime / _openDuration);
            _lidTransform.localRotation = Quaternion.Slerp(_initialRotation, _targetRotation, t);

            yield return null;
        }

        _lidTransform.localRotation = _targetRotation;
        _isOpening = false;
    }
}
