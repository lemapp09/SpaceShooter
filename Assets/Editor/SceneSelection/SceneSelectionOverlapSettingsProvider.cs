using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SceneSelectionOverlapSettingsProvider : SettingsProvider
    {
        public SceneSelectionOverlapSettingsProvider(string path, SettingsScope scope)
            : base(path, scope)
        {
        }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            GUILayout.Space(20f);

            bool enabled = SceneSelectionOverlaySettings.instance.AdditiveOptionEnabled;
            bool value = EditorGUILayout.Toggle("Additive Option", enabled, GUILayout.Width(200f));
            if (enabled != value)
                SceneSelectionOverlaySettings.instance.AdditiveOptionEnabled = value;
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new SceneSelectionOverlapSettingsProvider("Tools/Scene Selection Overlay", SettingsScope.Project);
        }

    }
}
