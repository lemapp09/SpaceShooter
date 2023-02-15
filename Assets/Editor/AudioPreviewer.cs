using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static class AudioPreviewer
{
    private static int? ms_lastPlayedAudioClipID = null;
    
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceId);

        if (obj is AudioClip audioClip)
        {
            if (IsPreviewClipPlaying())
            {
                StopAllPreviewClips();
                if(ms_lastPlayedAudioClipID.HasValue && ms_lastPlayedAudioClipID != audioClip.GetInstanceID())
                    PlayPreviewClip(audioClip);
            }
            else
            {
                PlayPreviewClip(audioClip);
            }
            
            ms_lastPlayedAudioClipID = audioClip.GetInstanceID();
            
            return true;
        }
        
        return false;
    }

    public static void PlayPreviewClip(AudioClip audioClip)
    {
        Assembly unityAssembly = typeof(AudioImporter).Assembly;
        Type audioutil = unityAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo methodInfo = audioutil.GetMethod(
            "PlayPreviewClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] { typeof(AudioClip), typeof(Int32), typeof(Boolean) },
            null);
        methodInfo.Invoke(null, new object[] { audioClip, 0, false });
        
    }
    
    public static bool IsPreviewClipPlaying()
    {
        Assembly unityAssembly = typeof(AudioImporter).Assembly;
        Type audioutil = unityAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo methodInfo = audioutil.GetMethod(
            "IsPreviewClipPlaying",
            BindingFlags.Static | BindingFlags.Public);

        return (bool)methodInfo.Invoke(null, null);
    }
    
    public static void StopAllPreviewClips()
    {
        Assembly unityAssembly = typeof(AudioImporter).Assembly;
        Type audioutil = unityAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo methodInfo = audioutil.GetMethod(
            "StopAllPreviewClips",
            BindingFlags.Static | BindingFlags.Public);

        methodInfo.Invoke(null, null);
    }

}

