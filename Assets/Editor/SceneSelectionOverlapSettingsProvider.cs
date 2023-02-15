using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SceneSelectionOverlapSettingsProvider : SettingsProvider
    {
        private const string K_additiveOptionPref = "ShowAdditiveSceneOption";

        public static bool AdditiveOptionEnabled
        {
            get { return EditorPrefs.GetBool(K_additiveOptionPref, true); }
            set { EditorPrefs.SetBool(K_additiveOptionPref, value); }
        }

        public SceneSelectionOverlapSettingsProvider(string path, SettingsScope scope)
            : base(path, scope)
        {
        }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            GUILayout.Space(20f);

            bool enabled = AdditiveOptionEnabled;
            bool value = EditorGUILayout.Toggle("Additive Option", enabled, GUILayout.Width(200f));
            if (enabled != value)
                AdditiveOptionEnabled = value;
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new SceneSelectionOverlapSettingsProvider("Tools/Scene Selection Overlay", SettingsScope.User);
        }

    }
}
