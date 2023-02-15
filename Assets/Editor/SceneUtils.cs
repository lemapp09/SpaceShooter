using System.Collections.Generic;
using UnityEditor;

public class SceneUtils 
{
    public static string[] GetEnabledScenes()
    {
        // Get the array of scenes included in the build
        var scenes = EditorBuildSettings.scenes;
        
        // Create a list to0 hold the names of the enabled scenes
        var enabledScenes = new List<string>();
        
        // Loop through the scenes and add the names of any enabled scenes to the list.
        foreach (var scene in scenes)
        {
            if (scene.enabled)
            {
                enabledScenes.Add(scene.path);
            }
        }
        
        // Return the list of enabled scene names as an array
        return enabledScenes.ToArray();
    }
}
/*
 *    You can use this function in your Unity editor script by calling
 *    'SceneUtils.GetEnabledScenes()', which will return an array of strings containing the paths
 *    of all enabled scenes in the build.
*/