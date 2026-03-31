using System;
using System.Collections.Generic;

namespace Fisherman.Core
{
    /// <summary>
    /// Simple publish/subscribe event bus for decoupled communication.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<Delegate>();
            _subscribers[type].Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_subscribers.ContainsKey(type))
                _subscribers[type].Remove(handler);
        }

        public static void Publish<T>(T eventData)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type)) return;

            // Iterate copy to allow modifications during dispatch
            var handlers = new List<Delegate>(_subscribers[type]);
            foreach (var handler in handlers)
            {
                (handler as Action<T>)?.Invoke(eventData);
            }
        }

        public static void Clear()
        {
            _subscribers.Clear();
        }
    }

    // ===== Game Events =====

    public struct GameStartedEvent { }

    public struct CastStartedEvent
    {
        public float Power;
        public float Angle;
    }

    public struct CastLandedEvent
    {
        public UnityEngine.Vector3 LandPosition;
    }

    public struct BiteEvent
    {
        public bool IsFakeBite;
    }

    public struct HookResultEvent
    {
        public bool Success;
    }

    public struct FightUpdateEvent
    {
        public float Tension;       // 0-1
        public float FishStamina;   // 0-1
        public float FishDirection; // -1 left, 0 center, 1 right
    }

    public struct CatchResultEvent
    {
        public bool Success;
        public FishData Fish;
        public int CoinsEarned;
    }

    public struct FishEscapedEvent
    {
        public string Reason; // "line_break" or "unhook"
    }

    public struct CoinsChangedEvent
    {
        public int NewTotal;
        public int Delta;
    }

    public struct EquipmentUpgradedEvent
    {
        public string EquipmentId;
        public int NewLevel;
    }

    public struct UINavigateEvent
    {
        public string TargetScreen; // "home", "fishing", "shop", "result"
    }
}
