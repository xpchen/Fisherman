using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fisherman.Core;

namespace Fisherman.UI
{
    /// <summary>
    /// Simple shop for upgrading the fishing rod.
    /// MVP only has one upgrade: Basic Rod -> Enhanced Rod.
    /// </summary>
    public class ShopPanel : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _panel;

        [Header("Equipment")]
        [SerializeField] private EquipmentData _rodData;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _rodNameText;
        [SerializeField] private TextMeshProUGUI _rodDescText;
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        [SerializeField] private TextMeshProUGUI _upgradeCostText;
        [SerializeField] private TextMeshProUGUI _upgradeEffectText;
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TextMeshProUGUI _upgradeButtonText;
        [SerializeField] private Button _backButton;

        [Header("Funny")]
        [SerializeField] private TextMeshProUGUI _funnyMessageText;

        private static readonly string[] UpgradeSuccessTexts = {
            "升级成功！鱼们开始颤抖了！",
            "新鱼竿到手！这下鱼跑不了了！",
            "恭喜晋级！从菜鸟变成了...高级菜鸟！",
        };

        private static readonly string[] NotEnoughTexts = {
            "穷！先去多钓几条鱼吧！",
            "金币不够...鱼在嘲笑你",
            "钱到用时方恨少啊...",
        };

        private static readonly string[] MaxLevelTexts = {
            "已经是最强了！（暂时）",
            "满级了！等下个版本再来吧",
        };

        private void OnEnable()
        {
            EventBus.Subscribe<UINavigateEvent>(OnNavigate);
            EventBus.Subscribe<CoinsChangedEvent>(OnCoinsChanged);
            if (_upgradeButton) _upgradeButton.onClick.AddListener(OnUpgrade);
            if (_backButton) _backButton.onClick.AddListener(OnBack);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<UINavigateEvent>(OnNavigate);
            EventBus.Unsubscribe<CoinsChangedEvent>(OnCoinsChanged);
            if (_upgradeButton) _upgradeButton.onClick.RemoveListener(OnUpgrade);
            if (_backButton) _backButton.onClick.RemoveListener(OnBack);
        }

        private void Start()
        {
            if (_panel) _panel.SetActive(false);
        }

        private void OnNavigate(UINavigateEvent e)
        {
            if (e.TargetScreen == "shop")
            {
                if (_panel) _panel.SetActive(true);
                RefreshUI();
            }
        }

        private void RefreshUI()
        {
            if (GameManager.Instance == null || _rodData == null) return;

            var economy = GameManager.Instance.EconomySystem;
            int level = economy.RodLevel;

            if (_rodNameText) _rodNameText.text = _rodData.EquipmentName;
            if (_rodDescText) _rodDescText.text = _rodData.FunnyDescription;
            if (_currentLevelText) _currentLevelText.text = $"当前等级: {level}";
            if (_coinsText) _coinsText.text = $"{economy.Coins} 金币";

            bool isMaxLevel = level >= _rodData.MaxLevel;

            if (isMaxLevel)
            {
                if (_upgradeCostText) _upgradeCostText.text = "";
                if (_upgradeEffectText) _upgradeEffectText.text = "已满级！";
                if (_upgradeButtonText) _upgradeButtonText.text = "已满级";
                if (_upgradeButton) _upgradeButton.interactable = false;
            }
            else
            {
                if (_upgradeCostText) _upgradeCostText.text = $"升级费用: {_rodData.UpgradeCost} 金币";
                if (_upgradeEffectText)
                {
                    int nextLevel = level + 1;
                    _upgradeEffectText.text =
                        $"抛投距离 +{_rodData.GetCastDistanceBonus(nextLevel) * 100:F0}%\n" +
                        $"安全区间 +{_rodData.GetTensionSafeZoneBonus(nextLevel) * 100:F0}%\n" +
                        $"稀有鱼率 +{_rodData.GetRareFishChanceBonus(nextLevel) * 100:F0}%";
                }
                if (_upgradeButtonText) _upgradeButtonText.text = "升级！";
                if (_upgradeButton) _upgradeButton.interactable = economy.Coins >= _rodData.UpgradeCost;
            }
        }

        private void OnUpgrade()
        {
            if (GameManager.Instance == null) return;

            bool success = GameManager.Instance.EconomySystem.TryUpgradeRod(_rodData);

            if (_funnyMessageText)
            {
                if (success)
                    _funnyMessageText.text = UpgradeSuccessTexts[Random.Range(0, UpgradeSuccessTexts.Length)];
                else if (GameManager.Instance.EconomySystem.RodLevel >= _rodData.MaxLevel)
                    _funnyMessageText.text = MaxLevelTexts[Random.Range(0, MaxLevelTexts.Length)];
                else
                    _funnyMessageText.text = NotEnoughTexts[Random.Range(0, NotEnoughTexts.Length)];
            }

            RefreshUI();
        }

        private void OnBack()
        {
            if (_panel) _panel.SetActive(false);
            // Restart fishing after closing shop
            Object.FindFirstObjectByType<Fisherman.Fishing.FishingController>()?.RestartFishing();
        }

        private void OnCoinsChanged(CoinsChangedEvent e)
        {
            if (_coinsText) _coinsText.text = $"{e.NewTotal} 金币";
            RefreshUI();
        }
    }
}
