
using UnityEngine;
using UnityEditor;

public static class ScriptIconMenuItems
{
    const string k_label = "ScriptIcon";

    [MenuItem("Tools/Script Icons/ Assign Label")]
    static void AssignScriptIconMenuItem()
    {
        Object[] objects = Selection.objects;
        if (objects == null)
            return;

        foreach (Object obj in objects)
        {
            // AssetDatabase.SetLabels(obj, new string[] { k_label});

            string[] labels = AssetDatabase.GetLabels(obj);
            if (!ArrayUtility.Contains<string>(labels, k_label))
            {
                ArrayUtility.Add<string>(ref labels, k_label);
                AssetDatabase.SetLabels(obj, labels);
            }
        }
    }

    [MenuItem("Tools/Script Icons/Remove Label")]
    static void RemoveScriptMenuItem()
    {
        Object[] objects = Selection.objects;
        if (objects == null)
            return;

        foreach (Object obj in objects)
        {
            // AssetDatabase.ClearLabels(obj);
            
            string[] labels = AssetDatabase.GetLabels(obj);
            ArrayUtility.Remove<string>(ref labels, k_label);
            AssetDatabase.SetLabels(obj, labels);
        }
    }
    
    [MenuItem("Tools/Script Icons/Find")]
    static void FindScriptMenuItem()
    {
        string[] assetGuids = AssetDatabase.FindAssets($"t:texture2d l:{k_label}");

        foreach (string assetGuid in assetGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGuid);
            Debug.Log(path);
        }
    }

}
