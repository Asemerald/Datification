using UnityEditor;
using UnityEngine;
using Utils;

public class DebugMenu : BaseSingleton<DebugMenu>
{
    // Add a menu item next to the "Tools" menu
    [MenuItem("Debug/Show Singletons Debugs")]
    private static void ToggleSingletonsDebug()
    {
        // Toggle the SHOW_DEBUG flag in BaseSingleton
        BaseSingleton<MonoBehaviour>.SHOW_DEBUG = !BaseSingleton<MonoBehaviour>.SHOW_DEBUG;

        // Update the checkbox state in the menu
        Menu.SetChecked("Debug/Show Singletons Debugs", BaseSingleton<MonoBehaviour>.SHOW_DEBUG);
    }

    // Set the initial state of the checkbox when the editor starts
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        // Set the initial state of the checkbox based on the SHOW_DEBUG flag
        Menu.SetChecked("Debug/Show Singletons Debugs", BaseSingleton<MonoBehaviour>.SHOW_DEBUG);
    }
}

// Ça marche pas sa mère