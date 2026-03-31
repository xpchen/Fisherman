using UnityEngine;
using Fisherman.Core;

namespace Fisherman.Fishing
{
    /// <summary>
    /// Tension bar + fish struggle + player reel-in mechanic.
    /// Keep tension in the safe zone to tire out the fish.
    /// </summary>
    public class FightSystem : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private float _baseTensionDrain = 0.15f;    // Tension drops when not reeling
        [SerializeField] private float _reelTensionGain = 0.35f;     // Tension rises when reeling
        [SerializeField] private float _fishStaminaDrain = 0.08f;    // Fish tires over time
        [SerializeField] private float _rushInterval = 2.5f;         // Seconds between fish rushes
        [SerializeField] private float _rushTensionSpike = 0.3f;     // Tension jump on rush
        [SerializeField] private float _tensionDangerHigh = 0.85f;   // Line break zone
        [SerializeField] private float _tensionDangerLow = 0.15f;    // Unhook zone
        [SerializeField] private float _dangerDuration = 1.0f;       // Seconds in danger before fail

        private FishData _currentFish;
        private float _rushTimer;
        private float _dangerTimer;
        private float _totalFightTime;

        public float CurrentTension { get; private set; }
        public float FishStamina { get; private set; }
        public float FishDirection { get; private set; }
        public bool IsFishCaught { get; private set; }
        public bool IsFishEscaped { get; private set; }

        public void StartFight(FishData fish)
        {
            _currentFish = fish;
            CurrentTension = 0.5f;
            FishStamina = 1f;
            FishDirection = 0f;
            IsFishCaught = false;
            IsFishEscaped = false;
            _rushTimer = Random.Range(1f, _rushInterval);
            _dangerTimer = 0f;
            _totalFightTime = 0f;

            Debug.Log($"[Fight] Started fight with {fish?.FishName ?? "unknown fish"}! Difficulty: {fish?.FightDifficulty ?? 1f}");
        }

        public void UpdateFight(float deltaTime, bool isReeling)
        {
            if (IsFishCaught || IsFishEscaped) return;

            _totalFightTime += deltaTime;
            float difficulty = _currentFish?.FightDifficulty ?? 1f;

            // === Tension management ===
            if (isReeling)
            {
                CurrentTension += _reelTensionGain * deltaTime;
                FishStamina -= _fishStaminaDrain * deltaTime;
            }
            else
            {
                CurrentTension -= _baseTensionDrain * deltaTime;
            }

            // === Fish rush (periodic tension spike) ===
            _rushTimer -= deltaTime;
            if (_rushTimer <= 0f)
            {
                float rushStrength = _rushTensionSpike * (_currentFish?.RushStrength ?? 1f);
                CurrentTension += rushStrength;
                FishDirection = Random.value > 0.5f ? 1f : -1f;
                _rushTimer = _rushInterval / (_currentFish?.RushFrequency ?? 1f) + Random.Range(-0.5f, 0.5f);

                Debug.Log($"[Fight] Fish rushes {(FishDirection > 0 ? "right" : "left")}! Tension spike: +{rushStrength:F2}");
            }

            // Fish direction slowly returns to center
            FishDirection = Mathf.MoveTowards(FishDirection, 0f, deltaTime * 0.8f);

            // Clamp values
            CurrentTension = Mathf.Clamp01(CurrentTension);
            FishStamina = Mathf.Clamp01(FishStamina);

            // === Danger zone check ===
            bool inDanger = CurrentTension >= _tensionDangerHigh || CurrentTension <= _tensionDangerLow;
            if (inDanger)
            {
                _dangerTimer += deltaTime;
                if (_dangerTimer >= _dangerDuration)
                {
                    IsFishEscaped = true;
                    string reason = CurrentTension >= _tensionDangerHigh ? "line_break" : "unhook";
                    Debug.Log($"[Fight] Fish escaped! Reason: {reason}");
                    return;
                }
            }
            else
            {
                _dangerTimer = Mathf.Max(0f, _dangerTimer - deltaTime * 2f); // Recover faster
            }

            // === Win condition: fish stamina depleted ===
            if (FishStamina <= 0f)
            {
                IsFishCaught = true;
                Debug.Log($"[Fight] Fish caught! Fight lasted {_totalFightTime:F1}s");
            }
        }
    }
}
