using UnityEngine;

namespace Fisherman.Core
{
    [CreateAssetMenu(fileName = "NewEquipment", menuName = "Fisherman/Equipment Data")]
    public class EquipmentData : ScriptableObject
    {
        public string EquipmentId;
        public string EquipmentName;
        public Sprite Icon;

        [TextArea(2, 3)]
        public string FunnyDescription;

        [Header("Upgrade")]
        public int UpgradeCost = 150;
        public int MaxLevel = 1;

        [Header("Effects Per Level")]
        public float[] CastDistanceBonus = { 0f, 0.12f };       // +12% at level 1
        public float[] TensionSafeZoneBonus = { 0f, 0.10f };    // +10% at level 1
        public float[] RareFishChanceBonus = { 0f, 0.03f };     // +3% at level 1

        public float GetCastDistanceBonus(int level) =>
            level < CastDistanceBonus.Length ? CastDistanceBonus[level] : CastDistanceBonus[^1];

        public float GetTensionSafeZoneBonus(int level) =>
            level < TensionSafeZoneBonus.Length ? TensionSafeZoneBonus[level] : TensionSafeZoneBonus[^1];

        public float GetRareFishChanceBonus(int level) =>
            level < RareFishChanceBonus.Length ? RareFishChanceBonus[level] : RareFishChanceBonus[^1];
    }
}
