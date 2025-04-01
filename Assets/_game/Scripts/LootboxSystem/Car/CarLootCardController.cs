using UnityEngine;

public class CarLootCardController : MonoBehaviour
{
    private const string Loot = "Loot";

    [SerializeField] private Camera _renderCamera;

    private GameObject _currentSphere;
    private int _originalLayer;
    private int _lootLayer;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _lootLayer = LayerMask.NameToLayer(Loot);
        _renderCamera.gameObject.SetActive(false);
        _renderCamera.cullingMask = 1 << _lootLayer;
    }

    public void ShowCard(GameObject car)
    {
        ActivateCar(car);
        gameObject.SetActive(true);
    }

    private void ActivateCar(GameObject sphere)
    {
        if (_currentSphere != null)
        {
            ResetPreviousCar();
        }

        if (sphere != null)
        {
            _currentSphere = sphere;
            _originalLayer = _currentSphere.layer;
            _currentSphere.layer = _lootLayer;
            _currentSphere.SetActive(true);
        }

        _renderCamera.gameObject.SetActive(true);
    }

    private void ResetPreviousCar()
    {
        _currentSphere.layer = _originalLayer;
        _currentSphere.SetActive(false);
        _currentSphere = null;
    }
}