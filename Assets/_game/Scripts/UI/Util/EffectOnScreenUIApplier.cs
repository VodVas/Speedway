using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class EffectOnScreenUIApplier : MonoBehaviour
{
    [Serializable]
    private class EffectSettings
    {
        public EffectSettings(float visibleDuration, int maxEffectsCount, Vector2 sizeVariation)
        {
            VisibleDuration = visibleDuration;
            MaxEffectsCount = maxEffectsCount;
            SizeVariation = sizeVariation;
        }

        [field: SerializeField] public float VisibleDuration { get; private set; }
        [field: SerializeField] public int MaxEffectsCount { get; private set; }
        [field: SerializeField] public Vector2 SizeVariation { get; private set; }
        [field: SerializeField] public Image[] Images { get; private set; } = new Image[1];

        public void ValidateArraySize()
        {
            if (Images == null || Images.Length != MaxEffectsCount)
            {
                Image[] newArr = new Image[MaxEffectsCount];

                if (Images != null)
                {
                    int copyLength = Mathf.Min(Images.Length, newArr.Length);

                    for (int i = 0; i < copyLength; i++)
                    {
                        newArr[i] = Images[i];
                    }
                }

                Images = newArr;
            }
        }
    }

    private const string InvalidDataError = "BulletHoleUI Некорректные данные в инспекторе!";
    private const string WarningNoImages = "BulletHoleUI Массив с эффектами не заполнен или меньше MaxEffectsCount!";

    [SerializeField] private RectTransform _targetImage = null;

    [SerializeField]
    private EffectSettings _bulletSettings = new EffectSettings(
        visibleDuration: 2f,
        maxEffectsCount: 5,
        sizeVariation: new Vector2(30f, 15f)
    );

    [SerializeField]
    private EffectSettings _dirtSettings = new EffectSettings(
        visibleDuration: 3f,
        maxEffectsCount: 3,
        sizeVariation: new Vector2(5f, 5f)
    );

    private int _currentBulletIndex = 0;
    private int _currentDirtIndex = 0;
    private bool _initialized = false;

    private void OnValidate()
    {
        if (_bulletSettings != null) _bulletSettings.ValidateArraySize();
        if (_dirtSettings != null) _dirtSettings.ValidateArraySize();
    }

    private void Awake()
    {
        ValidateSerializedData();
    }

    private void Start()
    {
        if (!_initialized)
        {
            InitializeEffects();
        }
    }

    public void ShowBulletHole()
    {
        ShowEffect(ref _currentBulletIndex, _bulletSettings);
    }

    public void ShowDirtSmudge()
    {
        ShowEffect(ref _currentDirtIndex, _dirtSettings);
    }

    private void ShowEffect(ref int currentIndex, EffectSettings settings)
    {
        if (!_initialized || settings.Images == null || settings.Images.Length == 0)
        {
            Debug.LogWarning("[BulletHoleUI] Попытка показать эффект, но массив Images пуст.");
            return;
        }

        int index = currentIndex;
        currentIndex = (currentIndex + 1) % settings.MaxEffectsCount;

        int safeIndex = index % settings.Images.Length;
        Image effectImage = settings.Images[safeIndex];

        if (effectImage == null) return;

        SetupEffect(effectImage.rectTransform, settings);
        effectImage.gameObject.SetActive(true);

        StartCoroutine(HideEffectAfterSeconds(effectImage, settings.VisibleDuration));
    }

    private void SetupEffect(RectTransform effectRect, EffectSettings settings)
    {
        if (_targetImage == null) return;

        float width = _targetImage.rect.width;
        float height = _targetImage.rect.height;

        effectRect.anchoredPosition = new Vector2(
            UnityEngine.Random.Range(0f, width),
            UnityEngine.Random.Range(0f, height)
        );

        float randomScale = UnityEngine.Random.Range(settings.SizeVariation.x, settings.SizeVariation.y);
        effectRect.localScale = Vector3.one * randomScale;
    }

    private IEnumerator HideEffectAfterSeconds(Image effectImage, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (effectImage != null)
        {
            effectImage.gameObject.SetActive(false);
        }
    }

    private void ValidateSerializedData()
    {
        bool invalid = (_targetImage == null
                        || _bulletSettings == null
                        || _dirtSettings == null
                        || _bulletSettings.Images == null
                        || _dirtSettings.Images == null
                        || _bulletSettings.Images.Length == 0
                        || _dirtSettings.Images.Length == 0);

        if (invalid)
        {
            Debug.LogError(InvalidDataError, this);
            enabled = false;
            return;
        }

        if (_bulletSettings.Images.Length < _bulletSettings.MaxEffectsCount ||
            _dirtSettings.Images.Length < _dirtSettings.MaxEffectsCount)
        {
            Debug.LogWarning(WarningNoImages, this);
            enabled = false;
            return;
        }

        _initialized = true;
    }

    private void InitializeEffects()
    {
        InitializeEffectGroup(_bulletSettings);
        InitializeEffectGroup(_dirtSettings);

        _initialized = true;
    }

    private void InitializeEffectGroup(EffectSettings settings)
    {
        if (settings.Images == null) return;

        for (int i = 0; i < settings.Images.Length; i++)
        {
            Image img = settings.Images[i];

            if (img != null)
            {
                img.gameObject.SetActive(false);
                img.rectTransform.localScale = Vector3.one;
            }
        }
    }
}