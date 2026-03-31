using UnityEngine;
using Fisherman.Core;

namespace Fisherman.Fishing
{
    /// <summary>
    /// Main fishing flow controller. Manages state transitions for the entire fishing loop.
    /// </summary>
    public class FishingController : MonoBehaviour
    {
        [Header("Sub Systems")]
        [SerializeField] private CastSystem _castSystem;
        [SerializeField] private BiteSystem _biteSystem;
        [SerializeField] private FightSystem _fightSystem;

        [Header("Config")]
        [SerializeField] private FishData[] _availableFish;

        public FishingState CurrentState { get; private set; } = FishingState.Idle;
        public FishData CurrentFish { get; private set; }

        private float _stateTimer;

        private void Start()
        {
            TransitionTo(FishingState.Idle);
        }

        private void Update()
        {
            _stateTimer += Time.deltaTime;

            switch (CurrentState)
            {
                case FishingState.Idle:
                    UpdateIdle();
                    break;
                case FishingState.AimCast:
                    UpdateAimCast();
                    break;
                case FishingState.Casting:
                    UpdateCasting();
                    break;
                case FishingState.WaitingBite:
                    UpdateWaitingBite();
                    break;
                case FishingState.BiteWindow:
                    UpdateBiteWindow();
                    break;
                case FishingState.Fighting:
                    UpdateFighting();
                    break;
                case FishingState.CatchSuccess:
                case FishingState.CatchFail:
                    // Handled by UI, wait for player input to restart
                    break;
            }
        }

        public void TransitionTo(FishingState newState)
        {
            Debug.Log($"[Fishing] {CurrentState} -> {newState}");
            CurrentState = newState;
            _stateTimer = 0f;

            switch (newState)
            {
                case FishingState.Idle:
                    OnEnterIdle();
                    break;
                case FishingState.AimCast:
                    OnEnterAimCast();
                    break;
                case FishingState.Casting:
                    OnEnterCasting();
                    break;
                case FishingState.WaitingBite:
                    OnEnterWaitingBite();
                    break;
                case FishingState.BiteWindow:
                    OnEnterBiteWindow();
                    break;
                case FishingState.Fighting:
                    OnEnterFighting();
                    break;
                case FishingState.CatchSuccess:
                    OnEnterCatchSuccess();
                    break;
                case FishingState.CatchFail:
                    OnEnterCatchFail();
                    break;
            }
        }

        // ========== IDLE ==========
        private void OnEnterIdle()
        {
            CurrentFish = null;
        }

        private void UpdateIdle()
        {
            // Transition to AimCast when player touches/holds the cast button
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                TransitionTo(FishingState.AimCast);
            }
        }

        // ========== AIM CAST ==========
        private void OnEnterAimCast()
        {
            _castSystem.StartCharging();
        }

        private void UpdateAimCast()
        {
            _castSystem.UpdateCharging(Time.deltaTime);

            // Release to cast
            if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                float power = _castSystem.GetCurrentPower();
                _castSystem.Release();
                EventBus.Publish(new CastStartedEvent { Power = power, Angle = 0f });
                TransitionTo(FishingState.Casting);
            }
        }

        // ========== CASTING ==========
        private void OnEnterCasting()
        {
            // Play cast animation
        }

        private void UpdateCasting()
        {
            // Wait for cast animation to finish (simulated with timer)
            if (_stateTimer > 1.0f)
            {
                EventBus.Publish(new CastLandedEvent { LandPosition = Vector3.forward * _castSystem.GetCurrentPower() * 10f });
                TransitionTo(FishingState.WaitingBite);
            }
        }

        // ========== WAITING BITE ==========
        private void OnEnterWaitingBite()
        {
            CurrentFish = PickRandomFish();
            _biteSystem.StartWaiting(CurrentFish);
        }

        private void UpdateWaitingBite()
        {
            _biteSystem.UpdateWaiting(Time.deltaTime);

            if (_biteSystem.HasBitOccurred)
            {
                EventBus.Publish(new BiteEvent { IsFakeBite = false });
                TransitionTo(FishingState.BiteWindow);
            }
        }

        // ========== BITE WINDOW ==========
        private void OnEnterBiteWindow()
        {
            // Player has 1.2 seconds to react
        }

        private void UpdateBiteWindow()
        {
            // Player taps to hook
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                EventBus.Publish(new HookResultEvent { Success = true });
                TransitionTo(FishingState.Fighting);
                return;
            }

            // Missed the window
            if (_stateTimer > 1.2f)
            {
                EventBus.Publish(new HookResultEvent { Success = false });
                TransitionTo(FishingState.Idle);
            }
        }

        // ========== FIGHTING ==========
        private void OnEnterFighting()
        {
            _fightSystem.StartFight(CurrentFish);
        }

        private void UpdateFighting()
        {
            bool isReeling = Input.GetMouseButton(0) || Input.touchCount > 0;
            _fightSystem.UpdateFight(Time.deltaTime, isReeling);

            EventBus.Publish(new FightUpdateEvent
            {
                Tension = _fightSystem.CurrentTension,
                FishStamina = _fightSystem.FishStamina,
                FishDirection = _fightSystem.FishDirection
            });

            if (_fightSystem.IsFishCaught)
            {
                TransitionTo(FishingState.CatchSuccess);
            }
            else if (_fightSystem.IsFishEscaped)
            {
                TransitionTo(FishingState.CatchFail);
            }
        }

        // ========== RESULTS ==========
        private void OnEnterCatchSuccess()
        {
            int price = CurrentFish.GetRandomPrice();
            GameManager.Instance.EconomySystem.AddCoins(price);
            GameManager.Instance.SaveSystem.Data.TotalFishCaught++;
            GameManager.Instance.SaveSystem.Data.TotalCastCount++;

            EventBus.Publish(new CatchResultEvent
            {
                Success = true,
                Fish = CurrentFish,
                CoinsEarned = price
            });

            Debug.Log($"[Fishing] Caught {CurrentFish.FishName}! +{price} coins. {CurrentFish.FunnyDescription}");
        }

        private void OnEnterCatchFail()
        {
            string reason = _fightSystem.CurrentTension >= 1f ? "line_break" : "unhook";
            GameManager.Instance.SaveSystem.Data.TotalCastCount++;

            EventBus.Publish(new FishEscapedEvent { Reason = reason });
            EventBus.Publish(new CatchResultEvent
            {
                Success = false,
                Fish = CurrentFish,
                CoinsEarned = 0
            });

            Debug.Log($"[Fishing] Fish escaped! Reason: {reason}");
        }

        /// <summary>
        /// Called by UI when player wants to fish again.
        /// </summary>
        public void RestartFishing()
        {
            TransitionTo(FishingState.Idle);
        }

        private FishData PickRandomFish()
        {
            if (_availableFish == null || _availableFish.Length == 0)
            {
                Debug.LogError("[Fishing] No fish configured!");
                return null;
            }

            // Weighted random selection
            float totalWeight = 0f;
            foreach (var fish in _availableFish)
                totalWeight += fish.SpawnWeight;

            float roll = Random.Range(0f, totalWeight);
            float cumulative = 0f;
            foreach (var fish in _availableFish)
            {
                cumulative += fish.SpawnWeight;
                if (roll <= cumulative) return fish;
            }

            return _availableFish[^1];
        }
    }
}
