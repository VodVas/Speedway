using UnityEngine;
using UnityEngine.UI;

//public class UIEffectPlayer : MonoBehaviour
//{
//    [SerializeField] private Image _targetImage;
//    [SerializeField] private Image[] _bulletHoleImages;
//    [SerializeField] private float _displayDuration = 2f;

//    private const int ImageCount = 5;
//    private float[] _timers;

//    private void Awake()
//    {
//        if (_targetImage == null || _bulletHoleImages == null || _bulletHoleImages.Length != ImageCount)
//        {
//            Debug.LogError("Target image or bullet hole images are not properly initialized.");
//            enabled = false;
//            return;
//        }

//        _timers = new float[ImageCount];

//        ResetTimers();
//        DeactivateAllImages();
//    }

//    private void Update()
//    {
//        for (int i = 0; i < ImageCount; i++)
//        {
//            if (_bulletHoleImages[i].gameObject.activeSelf)
//            {
//                _timers[i] -= Time.deltaTime;

//                if (_timers[i] <= 0)
//                {
//                    _bulletHoleImages[i].gameObject.SetActive(false);
//                }
//            }
//        }
//    }

//    public void PlayEffect()
//    {
//        for (int i = 0; i < ImageCount; i++)
//        {
//            if (!_bulletHoleImages[i].gameObject.activeSelf)
//            {
//                Debug.LogWarning("HandleAdditionalEffects");
//                //PlaceImageAtRandomPosition(_bulletHoleImages[i]);
//                _bulletHoleImages[i].gameObject.SetActive(true);
//                _timers[i] = _displayDuration;
//                break;
//            }
//        }
//    }

//    private void PlaceImageAtRandomPosition(Image image)
//    {
//        RectTransform targetRect = _targetImage.rectTransform;
//        RectTransform holeRect = image.rectTransform;

//        Vector2 randomPosition = new Vector2(
//            Random.Range(0, targetRect.rect.width - holeRect.rect.width),
//            Random.Range(0, targetRect.rect.height - holeRect.rect.height)
//        );

//        holeRect.anchoredPosition = randomPosition - targetRect.rect.size / 2 + holeRect.rect.size / 2;
//    }

//    private void ResetTimers()
//    {
//        for (int i = 0; i < ImageCount; i++)
//        {
//            _timers[i] = 0f;
//        }
//    }

//    private void DeactivateAllImages()
//    {
//        foreach (var image in _bulletHoleImages)
//        {
//            image.gameObject.SetActive(false);
//        }
//    }
//}