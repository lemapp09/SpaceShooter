using System.Linq;
using UnityEditor;
using UnityEngine;

public class Example2 
{
    [MenuItem("Edit/Reset Selected Ocjects Position")]
    static void ResetPosition()
    {
        var transforms = Selection.gameObjects.Select(go => go.transform).ToArray();
        var so = new SerializedObject(transforms);
        // you can Shift+Right Click on property names in the Inspector to see their paths
        so.FindProperty("m_LocalPosition").vector3Value = Vector3.zero;
        so.ApplyModifiedProperties();
    }
}
