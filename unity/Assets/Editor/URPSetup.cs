using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Fisherman.Editor
{
    public static class URPSetup
    {
        [MenuItem("Fisherman/Setup URP Pipeline")]
        public static void SetupURP()
        {
            string dir = "Assets/_Project/Settings";
            if (!AssetDatabase.IsValidFolder(dir))
                AssetDatabase.CreateFolder("Assets/_Project", "Settings");

            // Create URP Asset and Renderer
            string rendererPath = dir + "/FishermanRenderer.asset";
            string pipelinePath = dir + "/FishermanURP.asset";

            // Create renderer data
            var rendererData = ScriptableObject.CreateInstance<UniversalRendererData>();
            AssetDatabase.DeleteAsset(rendererPath);
            AssetDatabase.CreateAsset(rendererData, rendererPath);

            // Create pipeline asset using the internal creation method
            var pipelineAsset = UniversalRenderPipelineAsset.Create(rendererData);
            pipelineAsset.name = "FishermanURP";

            // Configure for mobile
            pipelineAsset.renderScale = 1f;
            pipelineAsset.supportsHDR = false;
            pipelineAsset.msaaSampleCount = 2;

            AssetDatabase.DeleteAsset(pipelinePath);
            AssetDatabase.CreateAsset(pipelineAsset, pipelinePath);
            AssetDatabase.SaveAssets();

            // Assign to Graphics Settings
            GraphicsSettings.defaultRenderPipeline = pipelineAsset;

            // Also assign to all quality levels
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                QualitySettings.SetQualityLevel(i, false);
                QualitySettings.renderPipeline = pipelineAsset;
            }

            // Switch to Linear color space for URP
            PlayerSettings.colorSpace = ColorSpace.Linear;

            AssetDatabase.SaveAssets();
            Debug.Log("[URPSetup] URP Pipeline configured! Restart may be required.");
        }
    }
}
