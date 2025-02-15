using UnityEngine;

public sealed class MinimapDisplay : MonoBehaviour
{
    private const int MinUpdateInterval = 1;

    [SerializeField] private RectTransform _mapRect = null;
    [SerializeField] private RectTransform[] _racerIcons = null;
    [SerializeField] private Transform[] _racerTransforms = null;
    [SerializeField] private Vector2 _scale = new Vector2(0.1f, 0.1f);

    private int _frameCounter = 0;

    [field: SerializeField] public int UpdateInterval { get; private set; } = 5;

    private void Awake()
    {
        if (_mapRect == null)
        {
            Debug.LogError("MinimapDisplay: _mapRect не назначен!", this);
            enabled = false;
            return;
        }

        if (_racerIcons == null || _racerIcons.Length == 0)
        {
            Debug.LogError("MinimapDisplay: Список иконок гонщиков пуст или не присвоен!", this);
            enabled = false;
            return;
        }

        if (_racerTransforms == null || _racerTransforms.Length != _racerIcons.Length)
        {
            Debug.LogError("MinimapDisplay: Список Transform гонщиков пуст, либо не совпадает по длине с иконками!", this);
            enabled = false;
            return;
        }

        if (_scale.x <= 0f || _scale.y <= 0f)
        {
            Debug.LogError("MinimapDisplay: Некорректный масштаб (_scale), должен быть больше 0!", this);
            enabled = false;
            return;
        }

        if (UpdateInterval < MinUpdateInterval)
        {
            Debug.LogError($"MinimapDisplay: UpdateInterval не может быть меньше {MinUpdateInterval}!", this);
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        _frameCounter = 0;
    }

    private void Start()
    {
        RefreshRacerPositions();
    }

    private void Update()
    {
        _frameCounter++;

        if (_frameCounter >= UpdateInterval)
        {
            RefreshRacerPositions();
            _frameCounter = 0;
        }
    }

    private void RefreshRacerPositions()
    {
        var halfWidth = _mapRect.rect.width * 0.5f;
        var halfHeight = _mapRect.rect.height * 0.5f;

        for (int i = 0; i < _racerTransforms.Length; i++)
        {
            var racerTransform = _racerTransforms[i];
            var icon = _racerIcons[i];

            if (racerTransform == null || icon == null)
            {
                continue;
            }

            var worldPos = racerTransform.position;
            float xPos = worldPos.x * _scale.x;
            float yPos = worldPos.z * _scale.y;

            xPos += halfWidth;
            yPos += halfHeight;

            icon.anchoredPosition = new Vector2(xPos, yPos);
        }
    }

    public void SetUpdateInterval(int newInterval)
    {
        if (newInterval < MinUpdateInterval)
        {
            Debug.LogError($"MinimapDisplay: SetUpdateInterval -> недопустимое значение {newInterval}!");
            enabled = false;
            return;
        }

        UpdateInterval = newInterval;
    }
}