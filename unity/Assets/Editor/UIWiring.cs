using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using Fisherman.UI;
using Fisherman.Fishing;

namespace Fisherman.Editor
{
    public static class UIWiring
    {
        [MenuItem("Fisherman/Wire UI References")]
        public static void WireReferences()
        {
            var canvas = GameObject.Find("Canvas");
            if (canvas == null) { Debug.LogError("[UIWiring] No Canvas!"); return; }

            WireFishingHUD(canvas);
            WireResultPanel(canvas);
            WireShopPanel(canvas);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            Debug.Log("[UIWiring] All UI references wired!");
        }

        static void WireFishingHUD(GameObject canvas)
        {
            var hudGO = canvas.transform.Find("FishingHUD")?.gameObject;
            if (hudGO == null) { Debug.LogError("[UIWiring] No FishingHUD!"); return; }

            var hud = hudGO.GetComponent<FishingHUD>();
            if (hud == null) { Debug.LogError("[UIWiring] No FishingHUD component!"); return; }

            var so = new SerializedObject(hud);

            // FishingController
            var fishingSystem = GameObject.Find("FishingSystem");
            if (fishingSystem != null)
                so.FindProperty("_fishingController").objectReferenceValue =
                    fishingSystem.GetComponent<FishingController>();

            // Cast UI
            var castPanel = hudGO.transform.Find("CastPanel")?.gameObject;
            so.FindProperty("_castPanel").objectReferenceValue = castPanel;

            if (castPanel != null)
            {
                var powerBar = castPanel.transform.Find("PowerBar")?.GetComponent<Slider>();
                so.FindProperty("_powerBar").objectReferenceValue = powerBar;

                var castHint = castPanel.transform.Find("CastHintText")?.GetComponent<TextMeshProUGUI>();
                so.FindProperty("_castHintText").objectReferenceValue = castHint;
            }

            // Tension UI
            var tensionPanel = hudGO.transform.Find("TensionPanel")?.gameObject;
            so.FindProperty("_tensionPanel").objectReferenceValue = tensionPanel;

            if (tensionPanel != null)
            {
                var tensionBar = tensionPanel.transform.Find("TensionBar")?.GetComponent<Slider>();
                so.FindProperty("_tensionBar").objectReferenceValue = tensionBar;

                var fishStaminaBar = tensionPanel.transform.Find("FishStaminaBar")?.GetComponent<Slider>();
                so.FindProperty("_fishStaminaBar").objectReferenceValue = fishStaminaBar;

                // TensionFill = the Fill Image inside TensionBar
                if (tensionBar != null)
                {
                    var fillImg = tensionBar.fillRect?.GetComponent<Image>();
                    so.FindProperty("_tensionFill").objectReferenceValue = fillImg;
                }
            }

            // Info UI
            so.FindProperty("_coinsText").objectReferenceValue =
                hudGO.transform.Find("CoinsText")?.GetComponent<TextMeshProUGUI>();
            so.FindProperty("_floatingText").objectReferenceValue =
                hudGO.transform.Find("FloatingText")?.GetComponent<TextMeshProUGUI>();
            so.FindProperty("_stateText").objectReferenceValue =
                hudGO.transform.Find("StateText")?.GetComponent<TextMeshProUGUI>();

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(hud);
            Debug.Log("[UIWiring] FishingHUD wired.");
        }

        static void WireResultPanel(GameObject canvas)
        {
            var panelGO = canvas.transform.Find("ResultPanel")?.gameObject;
            if (panelGO == null) return;

            var rp = panelGO.GetComponent<ResultPanel>();
            if (rp == null) return;

            var so = new SerializedObject(rp);
            so.FindProperty("_panel").objectReferenceValue = panelGO;

            var fishingSystem = GameObject.Find("FishingSystem");
            if (fishingSystem != null)
                so.FindProperty("_fishingController").objectReferenceValue =
                    fishingSystem.GetComponent<FishingController>();

            so.FindProperty("_resultTitleText").objectReferenceValue =
                panelGO.transform.Find("ResultTitleText")?.GetComponent<TextMeshProUGUI>();
            so.FindProperty("_fishNameText").objectReferenceValue =
                panelGO.transform.Find("FishNameText")?.GetComponent<TextMeshProUGUI>();
            so.FindProperty("_funnyDescriptionText").objectReferenceValue =
                panelGO.transform.Find("FunnyDescriptionText")?.GetComponent<TextMeshProUGUI>();
            so.FindProperty("_rarityText").objectReferenceValue =
                panelGO.transform.Find("RarityText")?.GetComponent<TextMeshProUGUI>();
            so.FindProperty("_coinsEarnedText").objectReferenceValue =
                panelGO.transform.Find("CoinsEarnedText")?.GetComponent<TextMeshProUGUI>();
            so.FindProperty("_fishImage").objectReferenceValue =
                panelGO.transform.Find("FishImage")?.GetComponent<Image>();

            var fishAgainBtn = panelGO.transform.Find("FishAgainButton")?.GetComponent<Button>();
            so.FindProperty("_fishAgainButton").objectReferenceValue = fishAgainBtn;

            var goShopBtn = panelGO.transform.Find("GoShopButton")?.GetComponent<Button>();
            so.FindProperty("_goShopButton").objectReferenceValue = goShopBtn;

            // Wire button text
            if (fishAgainBtn != null)
            {
                var txt = fishAgainBtn.GetComponentInChildren<TextMeshProUGUI>();
                if (txt != null) txt.text = "再钓一条！";
            }
            if (goShopBtn != null)
            {
                var txt = goShopBtn.GetComponentInChildren<TextMeshProUGUI>();
                if (txt != null) txt.text = "去商店";
            }

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(rp);
            Debug.Log("[UIWiring] ResultPanel wired.");
        }

        static void WireShopPanel(GameObject canvas)
        {
            var panelGO = canvas.transform.Find("ShopPanel")?.gameObject;
            if (panelGO == null) return;

            var sp = panelGO.GetComponent<ShopPanel>();
            if (sp == null) return;

            var so = new SerializedObject(sp);
            so.FindProperty("_panel").objectReferenceValue = panelGO;
            so.FindProperty("_currentLevelText").objectReferenceValue =
                panelGO.transform.Find("CurrentLevelText")?.GetComponent<TextMeshProUGUI>();
            so.FindProperty("_upgradeEffectText").objectReferenceValue =
                panelGO.transform.Find("UpgradeEffectText")?.GetComponent<TextMeshProUGUI>();
            so.FindProperty("_upgradeCostText").objectReferenceValue =
                panelGO.transform.Find("UpgradeCostText")?.GetComponent<TextMeshProUGUI>();
            so.FindProperty("_coinsText").objectReferenceValue =
                panelGO.transform.Find("ShopCoinsText")?.GetComponent<TextMeshProUGUI>();
            so.FindProperty("_funnyMessageText").objectReferenceValue =
                panelGO.transform.Find("UpgradeResultText")?.GetComponent<TextMeshProUGUI>();

            var upgradeBtn = panelGO.transform.Find("UpgradeButton")?.GetComponent<Button>();
            so.FindProperty("_upgradeButton").objectReferenceValue = upgradeBtn;

            var backBtn = panelGO.transform.Find("BackButton")?.GetComponent<Button>();
            so.FindProperty("_backButton").objectReferenceValue = backBtn;

            // Wire ShopTitleText
            var titleText = panelGO.transform.Find("ShopTitleText")?.GetComponent<TextMeshProUGUI>();
            if (titleText != null) titleText.text = "钓具商店";

            // Wire button texts
            var upgradeBtnText = panelGO.transform.Find("UpgradeButton")?.GetComponentInChildren<TextMeshProUGUI>();
            so.FindProperty("_upgradeButtonText").objectReferenceValue = upgradeBtnText;
            if (upgradeBtnText != null) upgradeBtnText.text = "升级！";

            if (backBtn != null)
            {
                var txt = backBtn.GetComponentInChildren<TextMeshProUGUI>();
                if (txt != null) txt.text = "返回";
            }

            // Wire rod data
            var rodData = AssetDatabase.LoadAssetAtPath<Fisherman.Core.EquipmentData>(
                "Assets/_Project/ScriptableObjects/Equipment_BasicRod.asset");
            so.FindProperty("_rodData").objectReferenceValue = rodData;

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(sp);
            Debug.Log("[UIWiring] ShopPanel wired.");
        }
    }
}
