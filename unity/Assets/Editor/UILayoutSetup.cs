using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

namespace Fisherman.Editor
{
    public static class UILayoutSetup
    {
        [MenuItem("Fisherman/Setup UI Layout")]
        public static void SetupLayout()
        {
            var canvas = GameObject.Find("Canvas");
            if (canvas == null) { Debug.LogError("[UILayout] No Canvas found!"); return; }

            // Fix FishingHUD - needs RectTransform
            var hudGO = canvas.transform.Find("FishingHUD")?.gameObject;
            if (hudGO == null) { Debug.LogError("[UILayout] No FishingHUD found!"); return; }

            // If FishingHUD doesn't have RectTransform, we need to recreate it
            var hudRT = hudGO.GetComponent<RectTransform>();
            if (hudRT == null)
            {
                var newHud = new GameObject("FishingHUD_New", typeof(RectTransform));
                newHud.transform.SetParent(canvas.transform, false);
                var newRT = newHud.GetComponent<RectTransform>();
                newRT.anchorMin = Vector2.zero;
                newRT.anchorMax = Vector2.one;
                newRT.sizeDelta = Vector2.zero;
                newRT.anchoredPosition = Vector2.zero;

                while (hudGO.transform.childCount > 0)
                    hudGO.transform.GetChild(0).SetParent(newHud.transform, false);

                int sibIndex = hudGO.transform.GetSiblingIndex();
                Object.DestroyImmediate(hudGO);
                newHud.name = "FishingHUD";
                newHud.transform.SetSiblingIndex(sibIndex);
                hudGO = newHud;
                hudRT = newRT;

                hudGO.AddComponent<Fisherman.UI.FishingHUD>();
                Debug.Log("[UILayout] Recreated FishingHUD with RectTransform");
            }
            else
            {
                hudRT.anchorMin = Vector2.zero;
                hudRT.anchorMax = Vector2.one;
                hudRT.sizeDelta = Vector2.zero;
                hudRT.anchoredPosition = Vector2.zero;
            }

            // ============ STATE TEXT - bottom center ============
            SetupText(hudGO, "StateText",
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0f, 80f), new Vector2(600f, 50f),
                24, TextAlignmentOptions.Center, new Color(1f, 1f, 1f, 0.9f));

            // ============ COINS TEXT - top right ============
            SetupText(hudGO, "CoinsText",
                new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(-80f, -40f), new Vector2(200f, 40f),
                28, TextAlignmentOptions.Right, new Color(1f, 0.85f, 0.2f, 1f));

            // ============ FLOATING TEXT - center ============
            SetupText(hudGO, "FloatingText",
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0f, 60f), new Vector2(700f, 60f),
                30, TextAlignmentOptions.Center, Color.white);

            // ============ CAST PANEL - bottom center ============
            var castPanel = SetupPanel(hudGO, "CastPanel",
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0f, 160f), new Vector2(400f, 80f));
            castPanel.SetActive(false);

            SetupSlider(castPanel, "PowerBar",
                new Vector2(0f, 0.5f), new Vector2(1f, 0.5f),
                new Vector2(0f, 10f), new Vector2(-40f, 30f),
                new Color(0.2f, 0.6f, 0.9f, 1f), new Color(0.15f, 0.15f, 0.15f, 0.7f));

            SetupText(castPanel, "CastHintText",
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0f, -5f), new Vector2(300f, 30f),
                18, TextAlignmentOptions.Center, new Color(1f, 1f, 1f, 0.7f));

            // ============ TENSION PANEL - center ============
            var tensionPanel = SetupPanel(hudGO, "TensionPanel",
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0f, -40f), new Vector2(500f, 120f));
            tensionPanel.SetActive(false);

            SetupSlider(tensionPanel, "TensionBar",
                new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0f, -20f), new Vector2(-40f, 25f),
                new Color(0.35f, 0.65f, 0.35f, 1f), new Color(0.15f, 0.15f, 0.15f, 0.7f));

            SetupSlider(tensionPanel, "FishStaminaBar",
                new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0f, -55f), new Vector2(-40f, 25f),
                new Color(0.9f, 0.5f, 0.2f, 1f), new Color(0.15f, 0.15f, 0.15f, 0.7f));

            // TensionFill label text
            SetupText(tensionPanel, "TensionFill",
                new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(0f, -85f), new Vector2(200f, 25f),
                16, TextAlignmentOptions.Center, new Color(1f, 1f, 1f, 0.6f));

            // ============ RESULT PANEL ============
            SetupResultPanel(canvas);

            // ============ SHOP PANEL ============
            SetupShopPanel(canvas);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            Debug.Log("[UILayout] UI layout setup complete!");
        }

        static void SetupText(GameObject parent, string name,
            Vector2 anchorMin, Vector2 anchorMax,
            Vector2 anchoredPos, Vector2 sizeDelta,
            float fontSize, TextAlignmentOptions alignment, Color color)
        {
            var t = parent.transform.Find(name);
            GameObject go;
            if (t != null)
            {
                go = t.gameObject;
                if (go.GetComponent<RectTransform>() == null)
                {
                    var newGO = new GameObject(name, typeof(RectTransform));
                    newGO.transform.SetParent(parent.transform, false);
                    Object.DestroyImmediate(go);
                    go = newGO;
                }
            }
            else
            {
                go = new GameObject(name, typeof(RectTransform));
                go.transform.SetParent(parent.transform, false);
            }

            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.anchoredPosition = anchoredPos;
            rt.sizeDelta = sizeDelta;

            var tmp = go.GetComponent<TextMeshProUGUI>();
            if (tmp == null) tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = fontSize;
            tmp.alignment = alignment;
            tmp.color = color;
            tmp.enableAutoSizing = false;

            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                "Assets/_Project/UI/Fonts/HeitiSC-Game-SDF.asset");
            if (fontAsset != null) tmp.font = fontAsset;

            EditorUtility.SetDirty(go);
        }

        static GameObject SetupPanel(GameObject parent, string name,
            Vector2 anchorMin, Vector2 anchorMax,
            Vector2 anchoredPos, Vector2 sizeDelta)
        {
            var t = parent.transform.Find(name);
            GameObject go;
            if (t != null)
            {
                go = t.gameObject;
                while (go.transform.childCount > 0)
                    Object.DestroyImmediate(go.transform.GetChild(0).gameObject);
            }
            else
            {
                go = new GameObject(name, typeof(RectTransform));
                go.transform.SetParent(parent.transform, false);
            }

            var rt = go.GetComponent<RectTransform>();
            if (rt == null)
            {
                var newGO = new GameObject(name, typeof(RectTransform));
                newGO.transform.SetParent(parent.transform, false);
                Object.DestroyImmediate(go);
                go = newGO;
                rt = go.GetComponent<RectTransform>();
            }
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.anchoredPosition = anchoredPos;
            rt.sizeDelta = sizeDelta;

            EditorUtility.SetDirty(go);
            return go;
        }

        static void SetupSlider(GameObject parent, string name,
            Vector2 anchorMin, Vector2 anchorMax,
            Vector2 anchoredPos, Vector2 sizeDelta,
            Color fillColor, Color bgColor)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Slider));
            go.transform.SetParent(parent.transform, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.anchoredPosition = anchoredPos;
            rt.sizeDelta = sizeDelta;

            var bgGO = new GameObject("Background", typeof(RectTransform), typeof(Image));
            bgGO.transform.SetParent(go.transform, false);
            var bgRT = bgGO.GetComponent<RectTransform>();
            bgRT.anchorMin = Vector2.zero;
            bgRT.anchorMax = Vector2.one;
            bgRT.sizeDelta = Vector2.zero;
            bgGO.GetComponent<Image>().color = bgColor;

            var fillAreaGO = new GameObject("Fill Area", typeof(RectTransform));
            fillAreaGO.transform.SetParent(go.transform, false);
            var fillAreaRT = fillAreaGO.GetComponent<RectTransform>();
            fillAreaRT.anchorMin = Vector2.zero;
            fillAreaRT.anchorMax = Vector2.one;
            fillAreaRT.offsetMin = new Vector2(5f, 0f);
            fillAreaRT.offsetMax = new Vector2(-5f, 0f);

            var fillGO = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            fillGO.transform.SetParent(fillAreaGO.transform, false);
            var fillRT = fillGO.GetComponent<RectTransform>();
            fillRT.anchorMin = Vector2.zero;
            fillRT.anchorMax = Vector2.one;
            fillRT.sizeDelta = Vector2.zero;
            fillGO.GetComponent<Image>().color = fillColor;

            var slider = go.GetComponent<Slider>();
            slider.fillRect = fillRT;
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 0.5f;
            slider.interactable = false;

            EditorUtility.SetDirty(go);
        }

        static void SetupResultPanel(GameObject canvas)
        {
            var t = canvas.transform.Find("ResultPanel");
            if (t == null) return;
            var panel = t.gameObject;
            var rt = panel.GetComponent<RectTransform>();
            if (rt == null) return;

            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;
            panel.SetActive(false);

            PositionChild(panel, "ResultBg", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var bgImg = panel.transform.Find("ResultBg")?.GetComponent<Image>();
            if (bgImg != null) bgImg.color = new Color(0f, 0f, 0f, 0.7f);

            PositionChild(panel, "FishImage",
                new Vector2(0.5f, 0.6f), new Vector2(0.5f, 0.6f),
                Vector2.zero, new Vector2(200f, 200f));

            PositionText(panel, "ResultTitleText", 0.5f, 0.85f, new Vector2(400f, 50f), 36);
            PositionText(panel, "FishNameText", 0.5f, 0.45f, new Vector2(400f, 40f), 28);
            PositionText(panel, "FunnyDescriptionText", 0.5f, 0.38f, new Vector2(500f, 40f), 22);
            PositionText(panel, "RarityText", 0.5f, 0.32f, new Vector2(300f, 35f), 22);
            PositionText(panel, "CoinsEarnedText", 0.5f, 0.26f, new Vector2(300f, 40f), 30);

            SetupButton(panel, "FishAgainButton", 0.35f, 0.12f, new Vector2(180f, 50f),
                new Color(0.35f, 0.65f, 0.35f, 1f));
            SetupButton(panel, "GoShopButton", 0.65f, 0.12f, new Vector2(180f, 50f),
                new Color(0.3f, 0.5f, 0.8f, 1f));

            EditorUtility.SetDirty(panel);
        }

        static void SetupShopPanel(GameObject canvas)
        {
            var t = canvas.transform.Find("ShopPanel");
            if (t == null) return;
            var panel = t.gameObject;
            var rt = panel.GetComponent<RectTransform>();
            if (rt == null) return;

            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;
            panel.SetActive(false);

            PositionChild(panel, "ShopBg", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var bgImg = panel.transform.Find("ShopBg")?.GetComponent<Image>();
            if (bgImg != null) bgImg.color = new Color(0.1f, 0.1f, 0.15f, 0.9f);

            PositionText(panel, "ShopTitleText", 0.5f, 0.9f, new Vector2(300f, 50f), 36);
            PositionText(panel, "CurrentLevelText", 0.5f, 0.7f, new Vector2(400f, 40f), 26);
            PositionText(panel, "UpgradeEffectText", 0.5f, 0.55f, new Vector2(450f, 80f), 22);
            PositionText(panel, "UpgradeCostText", 0.5f, 0.42f, new Vector2(400f, 40f), 24);
            PositionText(panel, "ShopCoinsText", 0.5f, 0.35f, new Vector2(300f, 40f), 24);
            PositionText(panel, "UpgradeResultText", 0.5f, 0.28f, new Vector2(500f, 40f), 22);

            SetupButton(panel, "UpgradeButton", 0.35f, 0.12f, new Vector2(180f, 50f),
                new Color(0.85f, 0.6f, 0.1f, 1f));
            SetupButton(panel, "BackButton", 0.65f, 0.12f, new Vector2(180f, 50f),
                new Color(0.5f, 0.5f, 0.5f, 1f));

            EditorUtility.SetDirty(panel);
        }

        static void PositionChild(GameObject parent, string name,
            Vector2 anchorMin, Vector2 anchorMax,
            Vector2 anchoredPos, Vector2 sizeDelta)
        {
            var t = parent.transform.Find(name);
            if (t == null) return;
            var rt = t.GetComponent<RectTransform>();
            if (rt == null) return;
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.anchoredPosition = anchoredPos;
            rt.sizeDelta = sizeDelta;
            EditorUtility.SetDirty(t.gameObject);
        }

        static void PositionText(GameObject parent, string name, float anchorX, float anchorY,
            Vector2 sizeDelta, float fontSize)
        {
            var t = parent.transform.Find(name);
            if (t == null) return;
            var rt = t.GetComponent<RectTransform>();
            if (rt == null) return;
            rt.anchorMin = new Vector2(anchorX, anchorY);
            rt.anchorMax = new Vector2(anchorX, anchorY);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = sizeDelta;

            var tmp = t.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.fontSize = fontSize;
                tmp.alignment = TextAlignmentOptions.Center;
                var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                    "Assets/_Project/UI/Fonts/HeitiSC-Game-SDF.asset");
                if (fontAsset != null) tmp.font = fontAsset;
            }
            EditorUtility.SetDirty(t.gameObject);
        }

        static void SetupButton(GameObject parent, string name, float anchorX, float anchorY,
            Vector2 sizeDelta, Color bgColor)
        {
            var t = parent.transform.Find(name);
            if (t == null) return;

            var rt = t.GetComponent<RectTransform>();
            if (rt == null) return;
            rt.anchorMin = new Vector2(anchorX, anchorY);
            rt.anchorMax = new Vector2(anchorX, anchorY);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = sizeDelta;

            var img = t.GetComponent<Image>();
            if (img != null) img.color = bgColor;

            var btnText = t.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                var btnTextRT = btnText.GetComponent<RectTransform>();
                btnTextRT.anchorMin = Vector2.zero;
                btnTextRT.anchorMax = Vector2.one;
                btnTextRT.sizeDelta = Vector2.zero;
                btnTextRT.anchoredPosition = Vector2.zero;
                btnText.alignment = TextAlignmentOptions.Center;
                btnText.fontSize = 22;
                btnText.color = Color.white;
            }
            EditorUtility.SetDirty(t.gameObject);
        }
    }
}
