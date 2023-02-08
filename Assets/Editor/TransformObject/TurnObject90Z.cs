using UnityEngine;
using UnityEditor;

namespace Editor.TransformObject
{
    public class TurnObject90Z : MonoBehaviour
    {
        [MenuItem("Tools/Transform Object/Rotate 90º Z   %&Z")]
        static void Turn()
        {
            Transform selectedTransform = Selection.activeTransform;
            if (selectedTransform != null)
            {
                selectedTransform.Rotate(0, 0, 90);
            }
        }
    }
}