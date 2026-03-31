using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Fisherman.Core;

namespace Fisherman.Editor
{
    public static class ArtWiring
    {
        private const string SpritePath = "Assets/_Project/UI/Sprites/";
        private const string TexturePath = "Assets/_Project/Textures/";
        private const string SOPath = "Assets/_Project/ScriptableObjects/";
        private const string MatPath = "Assets/_Project/Materials/";

        [MenuItem("Fisherman/Wire Art Assets")]
        public static void WireAll()
        {
            WireFishSprites();
            WireEquipmentIcons();
            WireMaterialTextures();
            WireUISprites();
            AssetDatabase.SaveAssets();
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            Debug.Log("[ArtWiring] All art assets wired!");
        }

        [MenuItem("Fisherman/Wire Fish Sprites")]
        public static void WireFishSprites()
        {
            WireFish("Fish_Crucian", "fish_crucian");
            WireFish("Fish_Bass", "fish_bass");
            WireFish("Fish_GoldenCarp", "fish_golden_carp");
            AssetDatabase.SaveAssets();
            Debug.Log("[ArtWiring] Fish sprites wired.");
        }

        static void WireFish(string assetName, string spriteBaseName)
        {
            var fishData = AssetDatabase.LoadAssetAtPath<FishData>(SOPath + assetName + ".asset");
            if (fishData == null)
            {
                Debug.LogWarning($"[ArtWiring] {assetName}.asset not found");
                return;
            }

            var so = new SerializedObject(fishData);

            TryAssignSprite(so, "FishSprite", SpritePath + spriteBaseName + ".png");
            TryAssignSprite(so, "ExpressionNormal", SpritePath + spriteBaseName + "_normal.png");
            TryAssignSprite(so, "ExpressionScared", SpritePath + spriteBaseName + "_scared.png");
            TryAssignSprite(so, "ExpressionSmug", SpritePath + spriteBaseName + "_smug.png");
            TryAssignSprite(so, "ExpressionSad", SpritePath + spriteBaseName + "_sad.png");

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(fishData);
        }

        [MenuItem("Fisherman/Wire Equipment Icons")]
        public static void WireEquipmentIcons()
        {
            var rod = AssetDatabase.LoadAssetAtPath<EquipmentData>(SOPath + "Equipment_BasicRod.asset");
            if (rod == null) return;

            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath + "icon_basic_rod.png");
            if (sprite != null)
            {
                var so = new SerializedObject(rod);
                so.FindProperty("Icon").objectReferenceValue = sprite;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(rod);
                Debug.Log("[ArtWiring] Equipment icon wired.");
            }
        }

        [MenuItem("Fisherman/Wire Material Textures")]
        public static void WireMaterialTextures()
        {
            WireMaterialTexture("Water.mat", "water_base.png");
            WireMaterialTexture("Ground.mat", "ground_base.png");
            AssetDatabase.SaveAssets();
            Debug.Log("[ArtWiring] Material textures wired.");
        }

        static void WireMaterialTexture(string matFile, string texFile)
        {
            var mat = AssetDatabase.LoadAssetAtPath<Material>(MatPath + matFile);
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(TexturePath + texFile);
            if (mat == null) { Debug.LogWarning($"[ArtWiring] Material {matFile} not found"); return; }
            if (tex == null) { Debug.LogWarning($"[ArtWiring] Texture {texFile} not found, skipping"); return; }

            mat.SetTexture("_BaseMap", tex);
            EditorUtility.SetDirty(mat);
        }

        [MenuItem("Fisherman/Wire UI Sprites")]
        public static void WireUISprites()
        {
            var canvas = GameObject.Find("Canvas");
            if (canvas == null) return;

            // Panel backgrounds
            WireImageSprite(canvas, "ResultPanel/ResultBg", "panel_bg.png");
            WireImageSprite(canvas, "ShopPanel/ShopBg", "panel_bg.png");

            // Buttons
            WireImageSprite(canvas, "ResultPanel/FishAgainButton", "btn_green.png");
            WireImageSprite(canvas, "ResultPanel/GoShopButton", "btn_blue.png");
            WireImageSprite(canvas, "ShopPanel/UpgradeButton", "btn_gold.png");
            WireImageSprite(canvas, "ShopPanel/BackButton", "btn_gray.png");

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            Debug.Log("[ArtWiring] UI sprites wired.");
        }

        static void WireImageSprite(GameObject root, string path, string spriteFile)
        {
            var t = root.transform.Find(path);
            if (t == null) return;
            var img = t.GetComponent<Image>();
            if (img == null) return;

            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath + spriteFile);
            if (sprite == null) return;

            img.sprite = sprite;
            img.type = Image.Type.Sliced;
            EditorUtility.SetDirty(img);
        }

        static void TryAssignSprite(SerializedObject so, string propertyName, string spritePath)
        {
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            if (sprite == null) return;

            var prop = so.FindProperty(propertyName);
            if (prop == null) return;

            prop.objectReferenceValue = sprite;
        }
    }
}
