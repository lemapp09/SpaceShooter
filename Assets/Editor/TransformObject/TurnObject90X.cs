using UnityEngine;
using UnityEditor;

namespace Editor.TransformObject
{
    public class TurnObject90X : MonoBehaviour
    {
        [MenuItem("Tools/Transform Object/Rotate 90º X  %&X")]
        static void Turn()
        {
            Transform selectedTransform = Selection.activeTransform;
            if (selectedTransform != null)
            {
                selectedTransform.Rotate(90, 0, 0);
            }
        }
    }
}