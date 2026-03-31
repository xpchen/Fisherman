using UnityEngine;
using Fisherman.Core;

namespace Fisherman.Core
{
    public class EconomySystem
    {
        private readonly SaveSystem _saveSystem;

        public int Coins => _saveSystem.Data.Coins;
        public int RodLevel => _saveSystem.Data.RodLevel;

        public EconomySystem(SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }

        public void AddCoins(int amount)
        {
            _saveSystem.Data.Coins += amount;
            EventBus.Publish(new CoinsChangedEvent
            {
                NewTotal = _saveSystem.Data.Coins,
                Delta = amount
            });
            Debug.Log($"[Economy] +{amount} coins! Total: {_saveSystem.Data.Coins}");
        }

        public bool TryUpgradeRod(EquipmentData equipment)
        {
            if (_saveSystem.Data.RodLevel >= equipment.MaxLevel)
            {
                Debug.Log("[Economy] Rod already at max level!");
                return false;
            }

            if (_saveSystem.Data.Coins < equipment.UpgradeCost)
            {
                Debug.Log($"[Economy] Not enough coins! Need {equipment.UpgradeCost}, have {_saveSystem.Data.Coins}");
                return false;
            }

            _saveSystem.Data.Coins -= equipment.UpgradeCost;
            _saveSystem.Data.RodLevel++;

            EventBus.Publish(new CoinsChangedEvent
            {
                NewTotal = _saveSystem.Data.Coins,
                Delta = -equipment.UpgradeCost
            });
            EventBus.Publish(new EquipmentUpgradedEvent
            {
                EquipmentId = equipment.EquipmentId,
                NewLevel = _saveSystem.Data.RodLevel
            });

            Debug.Log($"[Economy] Rod upgraded to level {_saveSystem.Data.RodLevel}!");
            return true;
        }
    }
}
