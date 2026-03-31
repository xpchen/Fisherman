using UnityEngine;
using UnityEditor;
using TMPro;

namespace Fisherman.Editor
{
    public static class FontSetup
    {
        [MenuItem("Fisherman/Create Chinese TMP Font")]
        public static void CreateChineseFont()
        {
            string fontPath = "Assets/_Project/UI/Fonts/HeitiSC-Game.ttf";
            Font sourceFont = AssetDatabase.LoadAssetAtPath<Font>(fontPath);
            if (sourceFont == null)
            {
                Debug.LogError($"[FontSetup] Font not found at {fontPath}");
                return;
            }

            TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(sourceFont);
            if (fontAsset == null)
            {
                Debug.LogError("[FontSetup] Failed to create TMP font asset!");
                return;
            }

            fontAsset.name = "HeitiSC-Game-SDF";

            string dir = "Assets/_Project/UI/Fonts";
            string sdfPath = dir + "/HeitiSC-Game-SDF.asset";
            AssetDatabase.DeleteAsset(sdfPath);
            AssetDatabase.CreateAsset(fontAsset, sdfPath);
            AssetDatabase.SaveAssets();

            Debug.Log($"[FontSetup] TMP font created at {sdfPath}");
            ApplyFontToScene(fontAsset);
        }

        [MenuItem("Fisherman/Apply Font To Scene")]
        public static void ApplyFontFromAsset()
        {
            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                "Assets/_Project/UI/Fonts/HeitiSC-Game-SDF.asset");
            if (fontAsset == null)
            {
                Debug.LogError("[FontSetup] Run 'Create Chinese TMP Font' first.");
                return;
            }
            ApplyFontToScene(fontAsset);
        }

        static void ApplyFontToScene(TMP_FontAsset fontAsset)
        {
            var tmpTexts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
            foreach (var tmp in tmpTexts)
            {
                tmp.font = fontAsset;
                tmp.fontSize = 24;
                EditorUtility.SetDirty(tmp);
            }

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            Debug.Log($"[FontSetup] Applied font to {tmpTexts.Length} TMP texts.");
        }
    }
}
