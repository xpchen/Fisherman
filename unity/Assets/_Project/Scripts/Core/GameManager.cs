using UnityEngine;

namespace Fisherman.Core
{
    /// <summary>
    /// Global game manager singleton. Persists across scenes.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("References")]
        public SaveSystem SaveSystem { get; private set; }
        public EconomySystem EconomySystem { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SaveSystem = new SaveSystem();
            EconomySystem = new EconomySystem(SaveSystem);
        }

        private void Start()
        {
            SaveSystem.Load();
            EventBus.Publish(new GameStartedEvent());
            Debug.Log("[GameManager] Game started! Coins: " + EconomySystem.Coins);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) SaveSystem.Save();
        }

        private void OnApplicationQuit()
        {
            SaveSystem.Save();
        }
    }
}
