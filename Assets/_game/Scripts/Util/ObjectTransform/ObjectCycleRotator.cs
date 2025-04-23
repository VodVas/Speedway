using UnityEngine;

public class ObjectCycleRotator : MonoBehaviour
{
    [SerializeField] private bool _ignoreTimescale = true;

    [field: SerializeField] public float SpeedX { get; private set; }
    [field: SerializeField] public float SpeedY { get; private set; }
    [field: SerializeField] public float SpeedZ { get; private set; }

    public Transform _cachedTransform { get; private set; }

    public bool _hasRotation { get; private set; }

    protected virtual void Start()
    {
        _cachedTransform = transform;
        UpdateRotationState();
    }

    protected virtual void Update()
    {
        if (_hasRotation)
        {
            Rotate(GetDeltaTime());
        }
    }

    public void SetRotation(bool rotation)
    {
        _hasRotation = rotation;
    }

    public void SetRotationSpeeds(float x, float y, float z)
    {
        SpeedX = x;
        SpeedY = y;
        SpeedZ = z;

        UpdateRotationState();
    }

    public float GetDeltaTime()
    {
        return _ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
    }

    private void UpdateRotationState()
    {
        _hasRotation = !Mathf.Approximately(SpeedX, 0f) || !Mathf.Approximately(SpeedY, 0f) || !Mathf.Approximately(SpeedZ, 0f);
    }

    private void Rotate(float deltaTime)
    {
        _cachedTransform.Rotate(SpeedX * deltaTime, SpeedY * deltaTime, SpeedZ * deltaTime);
    }
}