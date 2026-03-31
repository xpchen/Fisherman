using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fisherman.Core;
using Fisherman.Fishing;

namespace Fisherman.UI
{
    /// <summary>
    /// Shows catch result: fish display, funny text, coins earned, and action buttons.
    /// </summary>
    public class ResultPanel : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private FishingController _fishingController;

        [Header("Fish Display")]
        [SerializeField] private Image _fishImage;
        [SerializeField] private TextMeshProUGUI _fishNameText;
        [SerializeField] private TextMeshProUGUI _rarityText;
        [SerializeField] private TextMeshProUGUI _funnyDescriptionText;

        [Header("Result Info")]
        [SerializeField] private TextMeshProUGUI _coinsEarnedText;
        [SerializeField] private TextMeshProUGUI _resultTitleText;

        [Header("Buttons")]
        [SerializeField] private Button _fishAgainButton;
        [SerializeField] private Button _goShopButton;

        [Header("Colors")]
        [SerializeField] private Color _commonColor = Color.white;
        [SerializeField] private Color _uncommonColor = new Color(0.3f, 0.7f, 1f);
        [SerializeField] private Color _rareColor = new Color(1f, 0.84f, 0f);

        private void OnEnable()
        {
            EventBus.Subscribe<CatchResultEvent>(OnCatchResult);
            if (_fishAgainButton) _fishAgainButton.onClick.AddListener(OnFishAgain);
            if (_goShopButton) _goShopButton.onClick.AddListener(OnGoShop);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<CatchResultEvent>(OnCatchResult);
            if (_fishAgainButton) _fishAgainButton.onClick.RemoveListener(OnFishAgain);
            if (_goShopButton) _goShopButton.onClick.RemoveListener(OnGoShop);
        }

        private void Start()
        {
            if (_panel) _panel.SetActive(false);
        }

        private void OnCatchResult(CatchResultEvent e)
        {
            if (_panel) _panel.SetActive(true);

            if (e.Success && e.Fish != null)
            {
                ShowSuccess(e);
            }
            else
            {
                ShowFailure(e);
            }
        }

        private void ShowSuccess(CatchResultEvent e)
        {
            var fish = e.Fish;

            if (_resultTitleText) _resultTitleText.text = "上鱼啦！";
            if (_fishNameText) _fishNameText.text = fish.FishName;
            if (_fishImage && fish.FishSprite) _fishImage.sprite = fish.FishSprite;

            if (_rarityText)
            {
                _rarityText.text = fish.Rarity switch
                {
                    FishRarity.Common => "普通",
                    FishRarity.Uncommon => "少见",
                    FishRarity.Rare => "稀有！",
                    _ => ""
                };
                _rarityText.color = fish.Rarity switch
                {
                    FishRarity.Common => _commonColor,
                    FishRarity.Uncommon => _uncommonColor,
                    FishRarity.Rare => _rareColor,
                    _ => _commonColor
                };
            }

            if (_funnyDescriptionText)
                _funnyDescriptionText.text = FunnyTextSystem.GetCatchText(fish.Rarity);

            if (_coinsEarnedText)
                _coinsEarnedText.text = $"+{e.CoinsEarned} 金币";
        }

        private void ShowFailure(CatchResultEvent e)
        {
            if (_resultTitleText) _resultTitleText.text = "跑了...";
            if (_fishNameText) _fishNameText.text = e.Fish?.FishName ?? "神秘鱼";
            if (_funnyDescriptionText) _funnyDescriptionText.text = "下次一定！";
            if (_coinsEarnedText) _coinsEarnedText.text = "0 金币";
            if (_rarityText) _rarityText.text = "";
        }

        private void OnFishAgain()
        {
            if (_panel) _panel.SetActive(false);
            _fishingController?.RestartFishing();
        }

        private void OnGoShop()
        {
            if (_panel) _panel.SetActive(false);
            EventBus.Publish(new UINavigateEvent { TargetScreen = "shop" });
        }
    }
}
