using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;
using Editor;

[Overlay(typeof(SceneView), "Scene Selection")]
[Icon(k_icon)]
public class SceneSelectionOverlay : ToolbarOverlay
{
    public const string k_icon = "Assets/Editor/Icons/Scene.png";
    
    SceneSelectionOverlay() : base(SceneDropdownToggle.k_id) {}

    [EditorToolbarElement(k_id, typeof(SceneView))]
    class SceneDropdownToggle : EditorToolbarDropdownToggle, IAccessContainerWindow
    {
        public const string k_id = "SceneSelectionOverlay/SceneDropdownToggle";

        public EditorWindow containerWindow { get; set; }

        SceneDropdownToggle()
        {
            text = "Scenes";
            tooltip = "Select a scene to load";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>(SceneSelectionOverlay.k_icon);

            dropdownClicked += ShowSceneMenu;
        }

        private void ShowSceneMenu()
        {
            GenericMenu menu = new GenericMenu();

            Scene currentScene = EditorSceneManager.GetActiveScene();
            // var buildScenes = EditorBuildSettings.scenes;
            // if SceneUtils script is available, SceneUtils.GetEnabledScenes() 

            string[] sceneGuids = AssetDatabase.FindAssets("t:scene", null);

            for (int i = 0; i < sceneGuids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);

                string name = Path.GetFileNameWithoutExtension(path);

                if (string.Compare(currentScene.name, name) == 0)
                {
                    menu.AddDisabledItem(new GUIContent(name));
                }
                else
                if(SceneSelectionOverlapSettingsProvider.AdditiveOptionEnabled)
                {
                    menu.AddItem(new GUIContent(name + "/single"), false, () => OpenScene(currentScene, path, OpenSceneMode.Single));
                    menu.AddItem(new GUIContent(name + "/Additive"), false, () => OpenScene(currentScene, path, OpenSceneMode.Additive));
                }
                else
                {
                    menu.AddItem(new GUIContent(name), false, () => OpenScene(currentScene, path, OpenSceneMode.Single));
                }
                menu.AddItem(new GUIContent(name), string.Compare(currentScene.name, name) == 0,
                    () => OpenScene(currentScene, path, OpenSceneMode.Single));
            }
            menu.ShowAsContext();
        }
        void OpenScene(Scene currentScene, string path, OpenSceneMode openSceneMode)
        {
            if (currentScene.isDirty)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(path, openSceneMode);
            }
            else
            {
                EditorSceneManager.OpenScene(path, openSceneMode);
            }
        }
    }
}
