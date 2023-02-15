using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class MoveComponentTool
{
    private const string k_menuMoveToTop = "CONTEXT/Component/Move to Top";
    private const string k_menuMoveToBottom = "CONTEXT/Component/Move to Bottom";
    
    [MenuItem(k_menuMoveToTop, priority = 501)]
    public static void MoveComponentToTopMenuItem(MenuCommand command)
    {
        // Cycles through list of components until it reaches top
        while (UnityEditorInternal.ComponentUtility.MoveComponentUp((Component)command.context));
    }
    
    [MenuItem(k_menuMoveToTop, validate = true)]
    public static bool MoveComponentToTopMenuItemValidate(MenuCommand command)
    {
        // Greys out menu item if component is already on the top
        Component[] components = ((Component)command.context).gameObject.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == ((Component)command.context))
            {
                if (i == 1)
                    return false;
            }
        }
        return true;
    }
    
    [MenuItem(k_menuMoveToBottom, priority = 501)]
    public static void MoveComponentToBottomMenuItem(MenuCommand command)
    {
        // cycles through list of components until it reaches bottom
        while (UnityEditorInternal.ComponentUtility.MoveComponentDown((Component)command.context));
    }
  
    [MenuItem(k_menuMoveToBottom, validate = true)]
    public static bool MoveComponentToBottomMenuItemValidate(MenuCommand command)
    {
        // Greys out menu item if component is already on the bottom
        Component[] components = ((Component)command.context).gameObject.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == ((Component)command.context))
            {
                if (i == (components.Length - 1))
                    return false;
            }
        }
        return true;
    }
}
