using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField, Range(10, 120)] private int _targetFPS = 30;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = _targetFPS;
    }
}