using UnityEngine;
using YG;

public class OnlyDesktopParticlePlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private bool _isPlay;

    private void Awake()
    {
        if (YandexGame.EnvironmentData.isMobile && _isPlay)
        {
            _particleSystem.Play();
        }
    }
}

/*using System;
using System.Collections.Generic;
using UnityEngine;

namespace YG
{
    [Serializable]
    public class CarColorSave
    {
        public int carId;
        public int modificationId;
        public int selectedMaterialIndex;
    }

    [Serializable]
    public class PurchasedUpgrade
    {
        public int carId;
        public int upgradeId;
    }

    [Serializable]
    public class PurchasedModification
    {
        public int carId;
        public int modificationId;
        public int count;
    }

    [Serializable]
    public class SavesYG
    {
        private const int MaxModificationCount = 5;

        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        [SerializeField] private int _money = 10000;
        [SerializeField] private int _lastUsedCarId = -1;
        [SerializeField] private List<int> _purchasedCarIDs = new List<int>();
        [SerializeField] private List<PurchasedUpgrade> _purchasedUpgrades = new List<PurchasedUpgrade>();
        [SerializeField] private List<PurchasedModification> _purchasedModifications = new List<PurchasedModification>();
        [SerializeField] private List<CarColorSave> _carColorSaves = new List<CarColorSave>();
        [SerializeField] private List<int> _unlockedPaints = new List<int>();
        [SerializeField] private HashSet<int> _paintCache = new HashSet<int>();
        [SerializeField] private List<int> _unlockedEpicCars = new List<int>();
        [SerializeField] private bool[] _defeatedBosses = new bool[3];
        [SerializeField] private int _respect;

        public event Action OnMoneyChanged;
        public event Action<int> OnRespectChanged;

        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных
        }

        public int Money
        {
            get => _money;
            private set
            {
                _money = value;
                OnMoneyChanged?.Invoke();
            }
        }

        public int Respect
        {
            get => _respect;
            private set
            {
                _respect = value;
                OnRespectChanged?.Invoke(_respect);
            }
        }

        public bool IsBossDefeated(int bossIndex)
        {
            if (bossIndex < 0 || bossIndex >= _defeatedBosses.Length)
                return false;

            return _defeatedBosses[bossIndex];
        }

        public void SetBossDefeated(int bossIndex, bool defeated = true)
        {
            if (bossIndex < 0 || bossIndex >= _defeatedBosses.Length)
                return;

            _defeatedBosses[bossIndex] = defeated;
        }


        public int GetRespect() => _respect;

        public void AddRespect(int amount)
        {
            int newValue = _respect + amount;

            Respect = Mathf.Max(newValue, 0);
        }

        public void UnlockPaint(int paintId)
        {
            if (_paintCache.Add(paintId))
            {
                _unlockedPaints.Add(paintId);
            }
        }

        public bool IsPaintUnlocked(int paintId)
            => _paintCache.Contains(paintId);

        public int GetUnlockedPaintsCount()
            => _unlockedPaints.Count;

        public int GetUnlockedPaintId(int index)
            => _unlockedPaints[index];

        public void RefreshPaintCache()
        {
            if (_paintCache == null)
            {
                _paintCache = new HashSet<int>();
            }

            _paintCache.Clear();

            for (int i = 0; i < _unlockedPaints.Count; i++)
            {
                int paintId = _unlockedPaints[i];
                _paintCache.Add(paintId);
            }
        }

        public int GetSelectedMaterialIndex(int carId, int modificationId)
        {
            foreach (var save in _carColorSaves)
            {
                if (save.carId == carId && save.modificationId == modificationId)
                    return save.selectedMaterialIndex;
            }

            return 0;
        }

        public void SetSelectedMaterialIndex(int carId, int modificationId, int newIndex)
        {
            for (int i = 0; i < _carColorSaves.Count; i++)
            {
                var save = _carColorSaves[i];

                if (save.carId == carId && save.modificationId == modificationId)
                {
                    save.selectedMaterialIndex = newIndex;
                    _carColorSaves[i] = save;

                    return;
                }
            }
            _carColorSaves.Add(new CarColorSave
            {
                carId = carId,
                modificationId = modificationId,
                selectedMaterialIndex = newIndex
            });
        }

        public List<PurchasedUpgrade> GetUpgradeList()
        {
            return _purchasedUpgrades;
        }

        public List<PurchasedModification> GetModificationList()
        {
            return _purchasedModifications;
        }

        public int GetLastUsedCarId()
        {
            return _lastUsedCarId;
        }

        public void SetLastUsedCarId(int carId)
        {
            _lastUsedCarId = carId;
        }

        public void AddCar(int carId)
        {
            if (!_purchasedCarIDs.Contains(carId))
            {
                _purchasedCarIDs.Add(carId);
            }
        }

        public bool HasCar(int carId)
        {
            return _purchasedCarIDs.Contains(carId);
        }

        public void AddMoney(int amount)
        {
            if (amount < 0)
            {
                return;
            }

            Money += amount;
        }

        public bool TrySpendMoney(int amount)
        {
            if (amount < 0)
            {
                return false;
            }

            if (Money < amount)
            {
                return false;
            }

            Money -= amount;

            return true;
        }

        public bool HasCarUpgrade(int carId, int upgradeId)
        {
            foreach (var upgrade in _purchasedUpgrades)
            {
                if (upgrade.carId == carId && upgrade.upgradeId == upgradeId)
                    return true;
            }

            return false;
        }

        public void AddCarUpgrade(int carId, int upgradeId)
        {
            if (!HasCarUpgrade(carId, upgradeId))
            {
                var record = new PurchasedUpgrade { carId = carId, upgradeId = upgradeId };
                _purchasedUpgrades.Add(record);
            }
        }

        public int GetCarModificationCount(int carId, int modificationId)
        {
            foreach (var modification in _purchasedModifications)
            {
                if (modification.carId == carId && modification.modificationId == modificationId)
                {
                    return modification.count;
                }
            }

            return 0;
        }

        public void AddCarModification(int carId, int modificationId)
        {
            int currentCount = GetCarModificationCount(carId, modificationId);

            if (currentCount >= MaxModificationCount)
            {
                return;
            }

            bool found = false;

            for (int i = 0; i < _purchasedModifications.Count; i++)
            {
                if (_purchasedModifications[i].carId == carId &&
                    _purchasedModifications[i].modificationId == modificationId)
                {
                    var modification = _purchasedModifications[i];
                    modification.count++;
                    _purchasedModifications[i] = modification;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var newRecord = new PurchasedModification
                {
                    carId = carId,
                    modificationId = modificationId,
                    count = 1
                };
                _purchasedModifications.Add(newRecord);
            }
        }

        public void UnlockEpicCar(int carId)
        {
            if (!_unlockedEpicCars.Contains(carId))
            {
                _unlockedEpicCars.Add(carId);
            }
        }

        public void SaveUnlockedEpicCars(List<int> unlockedIds)
        {
            _unlockedEpicCars = new List<int>(unlockedIds);
        }

        public List<int> GetUnlockedEpicCars()
        {
            return new List<int>(_unlockedEpicCars);
        }
    }
}*/