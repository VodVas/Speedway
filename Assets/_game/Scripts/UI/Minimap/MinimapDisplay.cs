using System.Collections.Generic;
using UnityEngine;

public sealed class MinimapDisplay : MonoBehaviour
{
    private const int MIN_UPDATE_INTERVAL = 1;

    [SerializeField] private RectTransform _mapRect = null;
    [SerializeField] private Vector2 _scale = new Vector2(0.1f, 0.1f);
    [SerializeField] private int _updateInterval = 5;

    // ������ �������� � ������
    private readonly List<Transform> _racerTransforms = new List<Transform>();
    private readonly List<RectTransform> _racerIcons = new List<RectTransform>();

    private int _frameCounter;

    public MinimapDisplay() { }

    private void Awake()
    {
        if (_mapRect == null)
        {
            Debug.LogError("[MinimapDisplay] _mapRect �� ��������!", this);
            enabled = false;
            return;
        }

        if (_scale.x <= 0f || _scale.y <= 0f)
        {
            Debug.LogError("[MinimapDisplay] ������������ �������, ������ ���� > 0!", this);
            enabled = false;
            return;
        }

        if (_updateInterval < MIN_UPDATE_INTERVAL)
        {
            Debug.LogError($"[MinimapDisplay] _updateInterval �� ����� ���� ������ {MIN_UPDATE_INTERVAL}!");
            enabled = false;
            return;
        }
    }

    /// <summary>
    /// ���������������� ������ ������� �� ����� �������.
    /// </summary>
    /// <param name="racerTransform">Transform �������</param>
    /// <param name="racerIcon">������ �� UI</param>
    public void RegisterRacer(Transform racerTransform, RectTransform racerIcon)
    {
        if (racerTransform == null || racerIcon == null)
        {
            Debug.LogError("[MinimapDisplay] RegisterRacer ������� null!", this);
            enabled = false;
            return;
        }

        _racerTransforms.Add(racerTransform);
        _racerIcons.Add(racerIcon);
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
        if (_frameCounter >= _updateInterval)
        {
            RefreshRacerPositions();
            _frameCounter = 0;
        }
    }

    private void RefreshRacerPositions()
    {
        float halfWidth = _mapRect.rect.width * 0.5f;
        float halfHeight = _mapRect.rect.height * 0.5f;

        for (int i = 0; i < _racerTransforms.Count; i++)
        {
            Transform racerTransform = _racerTransforms[i];
            RectTransform icon = _racerIcons[i];

            if (racerTransform == null || icon == null)
            {
                continue;
            }

            Vector3 worldPos = racerTransform.position;
            float xPos = (worldPos.x * _scale.x) + halfWidth;
            float yPos = (worldPos.z * _scale.y) + halfHeight;

            icon.anchoredPosition = new Vector2(xPos, yPos);
        }
    }

    /// <summary>
    /// ��������� ��������� �������� ���������� ���������.
    /// </summary>
    public void SetUpdateInterval(int newInterval)
    {
        if (newInterval < MIN_UPDATE_INTERVAL)
        {
            Debug.LogError($"[MinimapDisplay] ������������ �������� ���������: {newInterval}");
            enabled = false;
            return;
        }

        _updateInterval = newInterval;
    }
}
















//public class MinimapDisplay : MonoBehaviour
//{
//    private const int MinUpdateInterval = 1;

//    [SerializeField] private RectTransform _mapRect = null;
//    [SerializeField] private RectTransform[] _racerIcons = null;
//    [SerializeField] private Transform[] _racerTransforms = null;
//    [SerializeField] private Vector2 _scale = new Vector2(0.1f, 0.1f);

//    private int _frameCounter = 0;

//    [field: SerializeField] public int UpdateInterval { get; private set; } = 5;

//    private void Awake()
//    {
//        if (_mapRect == null)
//        {
//            Debug.LogError("MinimapDisplay: _mapRect �� ��������!", this);
//            enabled = false;
//            return;
//        }

//        if (_racerIcons == null || _racerIcons.Length == 0)
//        {
//            Debug.LogError("MinimapDisplay: ������ ������ �������� ���� ��� �� ��������!", this);
//            enabled = false;
//            return;
//        }

//        if (_racerTransforms == null || _racerTransforms.Length != _racerIcons.Length)
//        {
//            Debug.LogError("MinimapDisplay: ������ Transform �������� ����, ���� �� ��������� �� ����� � ��������!", this);
//            enabled = false;
//            return;
//        }

//        if (_scale.x <= 0f || _scale.y <= 0f)
//        {
//            Debug.LogError("MinimapDisplay: ������������ ������� (_scale), ������ ���� ������ 0!", this);
//            enabled = false;
//            return;
//        }

//        if (UpdateInterval < MinUpdateInterval)
//        {
//            Debug.LogError($"MinimapDisplay: UpdateInterval �� ����� ���� ������ {MinUpdateInterval}!", this);
//            enabled = false;
//            return;
//        }
//    }

//    private void OnEnable()
//    {
//        _frameCounter = 0;
//    }

//    private void Start()
//    {
//        RefreshRacerPositions();
//    }

//    private void Update()
//    {
//        _frameCounter++;

//        if (_frameCounter >= UpdateInterval)
//        {
//            RefreshRacerPositions();
//            _frameCounter = 0;
//        }
//    }

//    private void RefreshRacerPositions()
//    {
//        var halfWidth = _mapRect.rect.width * 0.5f;
//        var halfHeight = _mapRect.rect.height * 0.5f;

//        for (int i = 0; i < _racerTransforms.Length; i++)
//        {
//            var racerTransform = _racerTransforms[i];
//            var icon = _racerIcons[i];

//            if (racerTransform == null || icon == null)
//            {
//                continue;
//            }

//            var worldPos = racerTransform.position;
//            float xPos = worldPos.x * _scale.x;
//            float yPos = worldPos.z * _scale.y;

//            xPos += halfWidth;
//            yPos += halfHeight;

//            icon.anchoredPosition = new Vector2(xPos, yPos);
//        }
//    }

//    public void SetUpdateInterval(int newInterval)
//    {
//        if (newInterval < MinUpdateInterval)
//        {
//            Debug.LogError($"MinimapDisplay: SetUpdateInterval -> ������������ �������� {newInterval}!");
//            enabled = false;
//            return;
//        }

//        UpdateInterval = newInterval;
//    }
//}