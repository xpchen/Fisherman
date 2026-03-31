using UnityEngine;

namespace Fisherman.Fishing
{
    /// <summary>
    /// Handles cast charging and power calculation.
    /// Long press to charge, release to cast.
    /// </summary>
    public class CastSystem : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private float _chargeSpeed = 0.6f; // Takes ~1.7s to full charge
        [SerializeField] private float _minPower = 0.15f;
        [SerializeField] private float _maxPower = 1.0f;

        private float _currentPower;
        private bool _isCharging;
        private bool _chargingUp = true; // Oscillates between 0 and 1

        public float CurrentPowerNormalized => _currentPower;
        public bool IsCharging => _isCharging;

        public void StartCharging()
        {
            _currentPower = _minPower;
            _isCharging = true;
            _chargingUp = true;
        }

        public void UpdateCharging(float deltaTime)
        {
            if (!_isCharging) return;

            // Power oscillates like a pendulum for satisfying timing
            if (_chargingUp)
            {
                _currentPower += _chargeSpeed * deltaTime;
                if (_currentPower >= _maxPower)
                {
                    _currentPower = _maxPower;
                    _chargingUp = false;
                }
            }
            else
            {
                _currentPower -= _chargeSpeed * deltaTime;
                if (_currentPower <= _minPower)
                {
                    _currentPower = _minPower;
                    _chargingUp = true;
                }
            }
        }

        public float GetCurrentPower()
        {
            return _currentPower;
        }

        public void Release()
        {
            _isCharging = false;
            Debug.Log($"[Cast] Released at power: {_currentPower:F2}");
        }
    }
}
