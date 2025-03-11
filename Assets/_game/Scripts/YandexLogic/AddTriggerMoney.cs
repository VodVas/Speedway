using UnityEngine;
using YG;

public class AddTriggerMoney : MonoBehaviour
{
    [SerializeField] private Transform _container;

    private Collider _playerCollider;
    private bool _hasGivenMoney;

    private void Start()
    {
        Player player = _container.GetComponentInChildren<Player>();

        if (player != null)
        {
            _playerCollider = player.GetComponent<Collider>();

            if (_playerCollider == null)
            {
                Debug.LogError("Player doesn't have a Collider component");
            }
        }
        else
        {
            Debug.LogError("Player not found in the container");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != _playerCollider || _hasGivenMoney || !_playerCollider.enabled)
            return;

        YandexGame.savesData.AddMoney(100);
        Debug.Log("YandexGame.savesData.AddMoney(100);");
        _hasGivenMoney = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _playerCollider)
            _hasGivenMoney = false;
    }

    private void OnDisable()
    {
        _hasGivenMoney = false;
    }
}