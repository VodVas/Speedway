using Cinemachine;
using UnityEngine;

public class CameraPrioritySwitcher : MonoBehaviour
{
    [SerializeField] private KeyCode _switchKey = KeyCode.C;
    [SerializeField] private VehicleMaterialStateApplier _materialApplier;
    [SerializeField] private CinemachineVirtualCamera[] _allCameras;

    private bool _currentState;
    private const int HIGH_PRIORITY = 20;
    private const int LOW_PRIORITY = 10;

    private void Update()
    {
        if (Input.GetKeyDown(_switchKey))
        {
            _currentState = !_currentState;
            UpdateActiveCamerasPriority();
            _materialApplier?.ApplyPreset(_currentState);
        }
    }

    private void UpdateActiveCamerasPriority()
    {
        if (_allCameras == null || _allCameras.Length < 2) return;

        var activeCameras = System.Array.FindAll(_allCameras, cam =>
            cam != null && cam.gameObject.activeInHierarchy);

        if (activeCameras.Length != 2)
        {
            Debug.LogWarning($"Need exactly 2 active cameras, found {activeCameras.Length}");
            return;
        }

        activeCameras[0].Priority = _currentState ? HIGH_PRIORITY : LOW_PRIORITY;
        activeCameras[1].Priority = _currentState ? LOW_PRIORITY : HIGH_PRIORITY;
    }

#if UNITY_EDITOR
    public void FindAllCameras()
    {
        _allCameras = FindObjectsOfType<CinemachineVirtualCamera>(true);
        _allCameras = System.Array.FindAll(_allCameras, cam => cam.gameObject.scene.IsValid());
    }

    public CinemachineVirtualCamera[] GetCameras() => _allCameras;
#endif
}