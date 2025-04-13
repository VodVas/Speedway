using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

[RequireComponent(typeof(Image))]
public class RespectProgressBarFiller : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private int _maxRespect = 100;

    private Image _fillImage;
    private int _currentRespect;

    private void Awake()
    {
        CacheComponents();
        //InitializeFillImage();
    }

    private void OnEnable()
    {
        YandexGame.savesData.OnRespectChanged += UpdateProgress;
        UpdateProgress(YandexGame.savesData.Respect);
    }

    private void OnDisable()
    {
        YandexGame.savesData.OnRespectChanged -= UpdateProgress;
    }

    private void CacheComponents()
    {
        _fillImage = GetComponent<Image>();
        if (_fillImage == null)
            Debug.LogError("Image component not found!", this);
    }

    private void InitializeFillImage()
    {
        _fillImage.type = Image.Type.Filled;
        //_fillImage.fillMethod = Image.FillMethod.Ver;
    }

    private void UpdateProgress(int currentRespect)
    {
        _currentRespect = Mathf.Clamp(currentRespect, 0, _maxRespect);
        _fillImage.fillAmount = (float)_currentRespect / _maxRespect;
    }
}