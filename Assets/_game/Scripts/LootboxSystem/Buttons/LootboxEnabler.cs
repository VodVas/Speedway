using UnityEngine;
using UnityEngine.UI;
using YG;

public class LootboxEnabler : MonoBehaviour
{
    [SerializeField] private GameObject[] _enableComponents;
    [SerializeField] private GameObject[] _disableComponent;
    [SerializeField] private Button _button;
    [SerializeField] private int _adID = 1;

    private void OnEnable()
    {
        _button.onClick.AddListener(Activate);
        YandexGame.RewardVideoEvent += Rewarded;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Activate);
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    private void Activate()
    {
        YandexGame.RewVideoShow(_adID);
    }

    private void Rewarded(int id)
    {
        if (id == _adID)
        {
            for (int i = 0; i < _enableComponents.Length; i++)
            {
                _enableComponents[i].SetActive(true);
            }

            for (int i = 0; i < _disableComponent.Length; i++)
            {
                _disableComponent[i].SetActive(false);
            }
        }
    }
}