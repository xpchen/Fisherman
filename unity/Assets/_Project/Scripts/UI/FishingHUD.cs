using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fisherman.Core;
using Fisherman.Fishing;

namespace Fisherman.UI
{
    /// <summary>
    /// Main fishing HUD - shows cast power, tension bar, coins, and floating text.
    /// Designed to be lightweight and not block the scenery.
    /// </summary>
    public class FishingHUD : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FishingController _fishingController;

        [Header("Cast UI")]
        [SerializeField] private GameObject _castPanel;
        [SerializeField] private Slider _powerBar;
        [SerializeField] private TextMeshProUGUI _castHintText;

        [Header("Tension UI")]
        [SerializeField] private GameObject _tensionPanel;
        [SerializeField] private Slider _tensionBar;
        [SerializeField] private Slider _fishStaminaBar;
        [SerializeField] private Image _tensionFill;

        [Header("Info UI")]
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private TextMeshProUGUI _floatingText;
        [SerializeField] private TextMeshProUGUI _stateText;

        [Header("Colors")]
        [SerializeField] private Color _tensionSafe = new Color(0.35f, 0.65f, 0.35f);
        [SerializeField] private Color _tensionDanger = new Color(0.85f, 0.2f, 0.2f);
        [SerializeField] private Color _tensionLow = new Color(0.9f, 0.7f, 0.2f);

        private float _floatingTextTimer;

        private void OnEnable()
        {
            EventBus.Subscribe<BiteEvent>(OnBite);
            EventBus.Subscribe<HookResultEvent>(OnHookResult);
            EventBus.Subscribe<FightUpdateEvent>(OnFightUpdate);
            EventBus.Subscribe<CatchResultEvent>(OnCatchResult);
            EventBus.Subscribe<CoinsChangedEvent>(OnCoinsChanged);
            EventBus.Subscribe<FishEscapedEvent>(OnFishEscaped);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<BiteEvent>(OnBite);
            EventBus.Unsubscribe<HookResultEvent>(OnHookResult);
            EventBus.Unsubscribe<FightUpdateEvent>(OnFightUpdate);
            EventBus.Unsubscribe<CatchResultEvent>(OnCatchResult);
            EventBus.Unsubscribe<CoinsChangedEvent>(OnCoinsChanged);
            EventBus.Unsubscribe<FishEscapedEvent>(OnFishEscaped);
        }

        private void Update()
        {
            if (_fishingController == null) return;

            var state = _fishingController.CurrentState;

            // Show/hide panels based on state
            if (_castPanel) _castPanel.SetActive(state == FishingState.AimCast);
            if (_tensionPanel) _tensionPanel.SetActive(state == FishingState.Fighting);

            // Update state hint text
            if (_stateText)
            {
                _stateText.text = state switch
                {
                    FishingState.Idle => FunnyTextSystem.GetIdleText(),
                    FishingState.AimCast => "蓄力中...",
                    FishingState.WaitingBite => FunnyTextSystem.GetWaitingText(),
                    FishingState.BiteWindow => "快！点击收竿！！",
                    FishingState.Fighting => "稳住！别让它跑！",
                    _ => ""
                };
            }

            // Floating text fade
            if (_floatingTextTimer > 0)
            {
                _floatingTextTimer -= Time.deltaTime;
                if (_floatingTextTimer <= 0 && _floatingText)
                    _floatingText.text = "";
            }
        }

        private void ShowFloatingText(string text, float duration = 2f)
        {
            if (_floatingText == null) return;
            _floatingText.text = text;
            _floatingTextTimer = duration;
        }

        // ===== Event Handlers =====

        private void OnBite(BiteEvent e)
        {
            if (e.IsFakeBite)
                ShowFloatingText("嗯？...假的！别急！", 1f);
            else
                ShowFloatingText(FunnyTextSystem.GetBiteText(), 1.5f);
        }

        private void OnHookResult(HookResultEvent e)
        {
            if (e.Success)
                ShowFloatingText(FunnyTextSystem.GetHookSuccessText());
            else
                ShowFloatingText("手慢了！鱼说：再见~", 2f);
        }

        private void OnFightUpdate(FightUpdateEvent e)
        {
            if (_tensionBar) _tensionBar.value = e.Tension;
            if (_fishStaminaBar) _fishStaminaBar.value = e.FishStamina;

            if (_tensionFill)
            {
                if (e.Tension > 0.85f)
                    _tensionFill.color = _tensionDanger;
                else if (e.Tension < 0.15f)
                    _tensionFill.color = _tensionLow;
                else
                    _tensionFill.color = _tensionSafe;
            }
        }

        private void OnCatchResult(CatchResultEvent e)
        {
            // Result screen will handle detailed display
            // HUD just shows a quick message
            if (e.Success)
                ShowFloatingText($"上鱼了！+{e.CoinsEarned} 金币!", 3f);
        }

        private void OnFishEscaped(FishEscapedEvent e)
        {
            ShowFloatingText(FunnyTextSystem.GetFailText(e.Reason), 3f);
        }

        private void OnCoinsChanged(CoinsChangedEvent e)
        {
            if (_coinsText)
                _coinsText.text = e.NewTotal.ToString();
        }
    }
}
