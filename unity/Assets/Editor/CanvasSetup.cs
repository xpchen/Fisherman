using UnityEngine;
using UnityEditor;

namespace Fisherman.Editor
{
    public static class CanvasSetup
    {
        [MenuItem("Fisherman/Set Canvas Camera Mode")]
        public static void SetCanvasCameraMode()
        {
            var canvas = GameObject.Find("Canvas")?.GetComponent<Canvas>();
            if (canvas == null) { Debug.LogError("[CanvasSetup] No Canvas!"); return; }

            var cam = Camera.main;
            if (cam == null) { Debug.LogError("[CanvasSetup] No Main Camera!"); return; }

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.worldCamera = null;

            EditorUtility.SetDirty(canvas);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            Debug.Log("[CanvasSetup] Canvas set to Screen Space - Camera mode.");
        }
    }
}
