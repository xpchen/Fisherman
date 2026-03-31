using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Fisherman.Editor
{
    public static class SliderFixup
    {
        [MenuItem("Fisherman/Fix Sliders")]
        public static void FixSliders()
        {
            // Find all Image components and reset their material to default
            var allImages = Resources.FindObjectsOfTypeAll<Image>();
            int fixed_count = 0;
            foreach (var img in allImages)
            {
                if (img.gameObject.scene.IsValid())
                {
                    // Reset material to default UI
                    img.material = null;
                    // Set default white color
                    if (img.color.a < 0.01f)
                        img.color = Color.white;
                    EditorUtility.SetDirty(img);
                    fixed_count++;
                }
            }

            // Also find all Sliders and ensure they have proper setup
            var sliders = Resources.FindObjectsOfTypeAll<Slider>();
            foreach (var slider in sliders)
            {
                if (!slider.gameObject.scene.IsValid()) continue;

                // Create Fill Area and Fill if missing
                var fillArea = slider.transform.Find("Fill Area");
                if (fillArea == null)
                {
                    var fillAreaGO = new GameObject("Fill Area", typeof(RectTransform));
                    fillAreaGO.transform.SetParent(slider.transform, false);
                    var fillAreaRT = fillAreaGO.GetComponent<RectTransform>();
                    fillAreaRT.anchorMin = Vector2.zero;
                    fillAreaRT.anchorMax = Vector2.one;
                    fillAreaRT.sizeDelta = Vector2.zero;

                    var fillGO = new GameObject("Fill", typeof(RectTransform), typeof(Image));
                    fillGO.transform.SetParent(fillAreaGO.transform, false);
                    var fillRT = fillGO.GetComponent<RectTransform>();
                    fillRT.anchorMin = Vector2.zero;
                    fillRT.anchorMax = Vector2.one;
                    fillRT.sizeDelta = Vector2.zero;

                    var fillImg = fillGO.GetComponent<Image>();
                    fillImg.color = new Color(0.35f, 0.65f, 0.35f, 1f);

                    slider.fillRect = fillRT;
                }

                // Create Background if missing
                var bg = slider.transform.Find("Background");
                if (bg == null)
                {
                    var bgGO = new GameObject("Background", typeof(RectTransform), typeof(Image));
                    bgGO.transform.SetParent(slider.transform, false);
                    bgGO.transform.SetAsFirstSibling();
                    var bgRT = bgGO.GetComponent<RectTransform>();
                    bgRT.anchorMin = Vector2.zero;
                    bgRT.anchorMax = Vector2.one;
                    bgRT.sizeDelta = Vector2.zero;

                    var bgImg = bgGO.GetComponent<Image>();
                    bgImg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                }

                EditorUtility.SetDirty(slider);
            }

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            Debug.Log($"[SliderFixup] Fixed {fixed_count} images and {sliders.Length} sliders.");
        }
    }
}
