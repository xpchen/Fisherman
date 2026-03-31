using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace Fisherman.Editor
{
    public static class MaterialSetup
    {
        [MenuItem("Fisherman/Setup Scene Materials")]
        public static void SetupMaterials()
        {
            // Use URP pipeline default material as source for proper keywords
            var rpAsset = GraphicsSettings.defaultRenderPipeline;
            Material sourceMat = rpAsset != null ? rpAsset.defaultMaterial : null;
            if (sourceMat == null)
            {
                Debug.LogError("[MaterialSetup] No URP default material found!");
                return;
            }

            CreateAndAssign("Water", "Assets/_Project/Materials/Water.mat",
                sourceMat, new Color(0.35f, 0.65f, 0.64f, 1f), 0.9f);

            CreateAndAssign("Ground", "Assets/_Project/Materials/Ground.mat",
                sourceMat, new Color(0.54f, 0.39f, 0.28f, 1f), 0.1f);

            AssetDatabase.SaveAssets();
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            Debug.Log("[MaterialSetup] Done!");
        }

        static void CreateAndAssign(string goName, string path, Material source, Color color, float smoothness)
        {
            AssetDatabase.DeleteAsset(path);
            var mat = new Material(source);
            mat.SetColor("_BaseColor", color);
            mat.SetFloat("_Smoothness", smoothness);
            mat.SetFloat("_Metallic", 0f);
            AssetDatabase.CreateAsset(mat, path);

            var go = GameObject.Find(goName);
            if (go != null)
                go.GetComponent<Renderer>().sharedMaterial = mat;
        }
    }
}
