using System;
using UnityEngine;

namespace Fisherman.Core
{
    public enum FishRarity
    {
        Common,
        Uncommon,
        Rare
    }

    [CreateAssetMenu(fileName = "NewFish", menuName = "Fisherman/Fish Data")]
    public class FishData : ScriptableObject
    {
        [Header("Basic Info")]
        public string FishId;
        public string FishName;
        public FishRarity Rarity;
        public Sprite FishSprite;

        [Header("Funny Description")]
        [TextArea(2, 4)]
        public string FunnyDescription;

        [Header("Gameplay")]
        [Range(0f, 1f)] public float SpawnWeight = 0.5f;
        [Range(0.5f, 3f)] public float FightDifficulty = 1f;
        public int MinPrice = 10;
        public int MaxPrice = 20;

        [Header("Fight Behavior")]
        public float StaminaMultiplier = 1f;
        public float RushFrequency = 1f;      // How often fish rushes
        public float RushStrength = 1f;        // How strong each rush is
        public float TensionSafeZoneModifier = 0f; // Shrinks safe zone

        [Header("Funny Expressions")]
        public Sprite ExpressionNormal;
        public Sprite ExpressionScared;
        public Sprite ExpressionSmug;
        public Sprite ExpressionSad;

        public int GetRandomPrice()
        {
            return UnityEngine.Random.Range(MinPrice, MaxPrice + 1);
        }
    }
}
