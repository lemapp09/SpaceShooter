using UnityEngine;
using UnityEditor;

namespace Editor.TransformObject
{
    public class TurnObject90Y : MonoBehaviour
    {
        [MenuItem("Tools/Transform Object/Rotate 90º Y  %&Y")]
        static void Turn()
        {
            Transform selectedTransform = Selection.activeTransform;
            if (selectedTransform != null)
            {
                selectedTransform.Rotate(0, 90, 0);
            }
        }
    }
}