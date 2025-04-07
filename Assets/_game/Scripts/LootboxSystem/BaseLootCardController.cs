using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class BaseLootCardController : MonoBehaviour
{
    private const string LootLayerName = "Loot";

    [SerializeField] private Camera _renderCamera;
    [SerializeField] private TextMeshProUGUI _rarityText;
    [SerializeField] private TextMeshProUGUI _epicRarityText;
    [SerializeField] private Image _cardBackground;

    private int _lootLayer;
    private int _originalLayer;
    private GameObject _currentObject;

    protected void AwakeBase()
    {
        _lootLayer = LayerMask.NameToLayer(LootLayerName);
        InitializeCamera();
    }

    private void InitializeCamera()
    {
        if (!_renderCamera) return;
        _renderCamera.gameObject.SetActive(false);
        _renderCamera.cullingMask = 1 << _lootLayer;
    }

    protected void ActivateObject(GameObject target)
    {
        ResetPreviousObject();

        if (!target) return;

        _currentObject = target;
        _originalLayer = _currentObject.layer;
        _currentObject.layer = _lootLayer;
        _currentObject.SetActive(true);

        SetCameraState(true);
    }

    protected void SetCameraState(bool active)
    {
        if (_renderCamera)
            _renderCamera.gameObject.SetActive(active);
    }

    protected void ConfigureRarityUI(Rarity rarity)
    {
        bool isEpic = rarity == Rarity.Epic;
        SetTextActive(_epicRarityText, isEpic);
        SetTextActive(_rarityText, !isEpic);
    }

    protected void SetText(string text)
    {
        if (_rarityText) _rarityText.text = text;
        if (_epicRarityText) _epicRarityText.text = text;
    }

    protected void SetBackground(Sprite sprite)
    {
        if (_cardBackground)
            _cardBackground.sprite = sprite;
    }

    private void SetTextActive(TextMeshProUGUI textElement, bool state)
    {
        if (textElement)
            textElement.gameObject.SetActive(state);
    }

    protected void ResetCard()
    {
        SetCameraState(false);
        gameObject.SetActive(false);
        ResetPreviousObject();
    }

    private void ResetPreviousObject()
    {
        if (!_currentObject) return;

        _currentObject.layer = _originalLayer;
        _currentObject.SetActive(false);
        _currentObject = null;
    }
}