using UnityEditor;
using UnityEngine;

namespace Editor
{
    
    // Go to "Edit > Select All GameObjects" to run this script and
    // select all game objects in the scene.
    //
    // This script use FindObjectsOfType method to find all game objects in the scene, and
    // then sets the 'Selection.objects' property to the array of game objects. This causes all
    // game objects to be selected in the editor.
    // 
    // Note: This script is intended to be used un Unity Editor, and it won't work when
    // running the game. It's also worth mentioning that this script use the 'MenuItem' attribute to
    // create a menu item in the Unity Editor's 'Edit' menu, so that you can easily run the script
    // from there.

    public class SelectAllGameObjects : UnityEditor.Editor
    {
        [MenuItem("Edit/Select All GameObjects %&a")]
        private static void SelectAll()
        {
            var gameObjects = GameObject.FindObjectsOfType<GameObject>();
            Selection.objects = gameObjects;
        }
    }
}