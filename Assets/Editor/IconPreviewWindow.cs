
using UnityEditor;
using UnityEngine;

public class IconPreviewWindow : EditorWindow
{
    [MenuItem("Tools/Icon Preview")]
    static void Init()
    {
        IconPreviewWindow window = (IconPreviewWindow)EditorWindow.GetWindow(typeof(IconPreviewWindow));
        window.Show();
    }

    private void OnGUI()
    {
        GUIContent content = EditorGUIUtility.IconContent("_Help@2x");
        GUILayout.Label(content);
        GUIContent content2 = EditorGUIUtility.IconContent("aboutwindow.mainheader");
        GUILayout.Label(content2);
    }
}

/*
    More internal Unity icons are listed here
    https://github.com/halak/unity-editor-icons/blob/master/README.md
    
*/