using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BulletHoleUI : MonoBehaviour
{
    private const int MaxHolesCount = 5;
    private const string InvalidDataError = "[BulletHoleUI] Некорректные данные в инспекторе!";
    private const string WarningNoImages = "[BulletHoleUI] Массив с дырками от пуль не заполнен!";

    [SerializeField] private RectTransform _targetImage = null;
    [SerializeField] private float _holeVisibleSeconds = 2f;
    [SerializeField] private Image[] _bulletHoleImages = null;

    private int _currentIndex = 0;
    private bool _initialized = false;

    private void Awake()
    {
        ValidateSerializedData();
    }

    private void Start()
    {
        if (_initialized == false)
        {
            InitializeHoles();
        }
    }

    public void ShowBulletHole()
    {
        if (!_initialized)
        {
            return;
        }

        int index = _currentIndex;
        _currentIndex = (_currentIndex + 1) % MaxHolesCount;

        Image hole = _bulletHoleImages[index];
        if (hole == null)
        {
            return;
        }

        PositionHoleRandomly(hole.rectTransform);
        hole.gameObject.SetActive(true);

        StartCoroutine(HideHoleAfterSeconds(hole, _holeVisibleSeconds));
    }

    private void ValidateSerializedData()
    {
        if (_targetImage == null || _bulletHoleImages == null || _bulletHoleImages.Length == 0)
        {
            Debug.LogError(InvalidDataError, this);
            enabled = false;
            return;
        }

        if (_bulletHoleImages.Length < 5)
        {
            Debug.LogWarning(WarningNoImages, this);
        }

        _initialized = true;
    }

    private void InitializeHoles()
    {
        for (int i = 0; i < _bulletHoleImages.Length; i++)
        {
            if (_bulletHoleImages[i] != null)
            {
                _bulletHoleImages[i].gameObject.SetActive(false);
            }
        }

        _initialized = true;
    }

    private void PositionHoleRandomly(RectTransform holeRect)
    {
        float x = Random.Range(0f, _targetImage.rect.width);
        float y = Random.Range(0f, _targetImage.rect.height);

        holeRect.anchoredPosition = new Vector2(x, y);
    }

    private IEnumerator HideHoleAfterSeconds(Image hole, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (hole != null)
        {
            hole.gameObject.SetActive(false);
        }
    }
}