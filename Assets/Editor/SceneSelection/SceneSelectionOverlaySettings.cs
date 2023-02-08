using UnityEditor;
using UnityEngine;

[FilePath("ProjectSettings/SceneSelectionOverlaySettings.asset", FilePathAttribute.Location.ProjectFolder)]
public class SceneSelectionOverlaySettings : ScriptableSingleton<SceneSelectionOverlaySettings>
{
   [SerializeField] 
   private bool m_additiveOptionEnabled = false;

   public bool AdditiveOptionEnabled {
      get => m_additiveOptionEnabled; 
      set { 
             m_additiveOptionEnabled = value;
             Save(true);  // true is text format, false is binary format
      }
   }
}
