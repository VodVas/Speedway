using UnityEngine;

internal sealed class Wallet : MonoBehaviour
{
    [SerializeField] private int _startMoney = 1500;

    private int _currentMoney;

    private void Awake()
    {
        _currentMoney = _startMoney;
    }

    public int CurrentMoney => _currentMoney;

    public bool TrySpendMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Wallet: сумма дл€ списани€ не может быть отрицательной!", this);
            return false;
        }

        if (_currentMoney >= amount)
        {
            _currentMoney -= amount;
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Wallet: сумма дл€ добавлени€ не может быть отрицательной!", this);
            return;
        }
        _currentMoney += amount;
    }
}