using UnityEngine;
using Fisherman.Core;

namespace Fisherman.Fishing
{
    /// <summary>
    /// Manages the waiting-for-bite phase. Handles timing, fake bites, and real bite trigger.
    /// </summary>
    public class BiteSystem : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private float _minWaitTime = 4f;
        [SerializeField] private float _maxWaitTime = 12f;
        [SerializeField] private float _fakeBiteChance = 0.3f;
        private float _waitTimer;
        private float _targetBiteTime;
        private bool _hasFakeBiteFired;
        private FishData _currentFish;

        public bool HasBitOccurred { get; private set; }

        public void StartWaiting(FishData fish)
        {
            _currentFish = fish;
            _waitTimer = 0f;
            _targetBiteTime = Random.Range(_minWaitTime, _maxWaitTime);
            _hasFakeBiteFired = false;
            HasBitOccurred = false;

            // Rare fish take slightly longer to bite (builds suspense)
            if (fish != null && fish.Rarity == FishRarity.Rare)
                _targetBiteTime *= 1.3f;

            Debug.Log($"[Bite] Waiting for bite... target time: {_targetBiteTime:F1}s");
        }

        public void UpdateWaiting(float deltaTime)
        {
            _waitTimer += deltaTime;

            // Optional fake bite at ~60% of wait time
            if (!_hasFakeBiteFired && _waitTimer > _targetBiteTime * 0.6f)
            {
                if (Random.value < _fakeBiteChance)
                {
                    _hasFakeBiteFired = true;
                    EventBus.Publish(new BiteEvent { IsFakeBite = true });
                    Debug.Log("[Bite] Fake bite! (bobber wiggle)");
                }
                else
                {
                    _hasFakeBiteFired = true; // Skip fake bite
                }
            }

            // Real bite
            if (_waitTimer >= _targetBiteTime)
            {
                HasBitOccurred = true;
                Debug.Log("[Bite] REAL BITE! React now!");
            }
        }
    }
}
