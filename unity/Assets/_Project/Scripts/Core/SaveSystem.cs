using System;
using System.IO;
using UnityEngine;

namespace Fisherman.Core
{
    [Serializable]
    public class SaveData
    {
        public int Coins;
        public int RodLevel; // 0 = basic, 1 = enhanced
        public int TotalFishCaught;
        public int TotalCastCount;
        public string[] CaughtFishIds = Array.Empty<string>();
    }

    public class SaveSystem
    {
        private const string SaveFileName = "fisherman_save.json";
        public SaveData Data { get; private set; } = new SaveData();

        private string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);

        public void Save()
        {
            try
            {
                var json = JsonUtility.ToJson(Data, true);
                File.WriteAllText(SavePath, json);
                Debug.Log("[SaveSystem] Saved to " + SavePath);
            }
            catch (Exception e)
            {
                Debug.LogError("[SaveSystem] Save failed: " + e.Message);
            }
        }

        public void Load()
        {
            try
            {
                if (File.Exists(SavePath))
                {
                    var json = File.ReadAllText(SavePath);
                    Data = JsonUtility.FromJson<SaveData>(json);
                    Debug.Log("[SaveSystem] Loaded save data.");
                }
                else
                {
                    Data = new SaveData();
                    Debug.Log("[SaveSystem] No save file found, starting fresh.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("[SaveSystem] Load failed: " + e.Message);
                Data = new SaveData();
            }
        }

        public void Reset()
        {
            Data = new SaveData();
            Save();
        }
    }
}
