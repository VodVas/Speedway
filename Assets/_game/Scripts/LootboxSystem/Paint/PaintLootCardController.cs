using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaintLootCardController : BaseLootCardController
{
    private void Awake() => AwakeBase();

    public void ShowCard(PaintLootItemSO item, GameObject sphere)
    {
        ConfigureVisuals(item);
        ActivateObject(sphere);
        gameObject.SetActive(true);
    }

    private void ConfigureVisuals(PaintLootItemSO item)
    {
        SetText(item.DisplayName);
        SetBackground(item.CardSprite);
        ConfigureRarityUI(item.Rarity);
    }

    public void HideCard() => ResetCard();
}





//public class PaintLootCardController : MonoBehaviour
//{
//    private const string Loot = "Loot";

//    [SerializeField] private TextMeshProUGUI _rarityText;
//    [SerializeField] private TextMeshProUGUI _epicRarityText;
//    [SerializeField] private Image _cardBackground;
//    [SerializeField] private Camera _renderCamera;

//    private GameObject _currentSphere;
//    private int _originalLayer;
//    private int _lootLayer;

//    private void Awake()
//    {
//        Initialize();
//    }

//    public void Initialize()
//    {
//        _lootLayer = LayerMask.NameToLayer(Loot);
//        _renderCamera.gameObject.SetActive(false);
//        _renderCamera.cullingMask = 1 << _lootLayer;
//    }

//    public void ShowCard(PaintLootItemSO item, GameObject sphere)
//    {
//        ConfigureVisuals(item);
//        ActivateSphere(sphere);
//        gameObject.SetActive(true);
//    }

//    private void ConfigureVisuals(PaintLootItemSO item)
//    {
//        _rarityText.text = item.DisplayName;
//        _epicRarityText.text = item.DisplayName;
//        _cardBackground.sprite = item.CardSprite;

//        if (item.Rarity == Rarity.Epic)
//        {
//            _rarityText.gameObject.SetActive(false);
//            _epicRarityText.gameObject.SetActive(true);
//        }
//        else
//        {
//            _rarityText.gameObject.SetActive(true);
//            _epicRarityText.gameObject.SetActive(false);
//        }
//    }

//    private void ActivateSphere(GameObject sphere)
//    {
//        if (_currentSphere != null)
//        {
//            ResetPreviousSphere();
//        }

//        if (sphere != null)
//        {
//            _currentSphere = sphere;
//            _originalLayer = _currentSphere.layer;
//            _currentSphere.layer = _lootLayer;
//            _currentSphere.SetActive(true);
//        }

//        _renderCamera.gameObject.SetActive(true);
//    }

//    private void ResetPreviousSphere()
//    {
//        if (_currentSphere == null) return;

//        _currentSphere.layer = _originalLayer;
//        _currentSphere.SetActive(false);
//        _currentSphere = null;
//    }

//    public void HideCard()
//    {
//        _renderCamera.gameObject.SetActive(false);
//        gameObject.SetActive(false);

//        if (_currentSphere != null)
//        {
//            ResetPreviousSphere();
//        }
//    }
//}