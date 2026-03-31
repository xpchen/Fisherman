using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Fisherman.Editor
{
    public static class UIFixup
    {
        [MenuItem("Fisherman/Fix UI Materials")]
        public static void FixUIMaterials()
        {
            // Fix all Image components to use default UI material
            var images = Object.FindObjectsByType<Image>(FindObjectsSortMode.None);
            int count = 0;
            foreach (var img in images)
            {
                if (img.material != null && img.material.shader.name != "UI/Default")
                {
                    img.material = null; // Reset to default UI material
                    EditorUtility.SetDirty(img);
                    count++;
                }
                // Also ensure sprite is set to null if it's using a broken sprite
                if (img.sprite != null && img.sprite.texture == null)
                {
                    img.sprite = null;
                    EditorUtility.SetDirty(img);
                }
            }

            // Fix all RawImage components
            var rawImages = Object.FindObjectsByType<RawImage>(FindObjectsSortMode.None);
            foreach (var ri in rawImages)
            {
                if (ri.material != null && ri.material.shader.name != "UI/Default")
                {
                    ri.material = null;
                    EditorUtility.SetDirty(ri);
                    count++;
                }
            }

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            Debug.Log($"[UIFixup] Fixed {count} UI elements.");
        }
    }
}
