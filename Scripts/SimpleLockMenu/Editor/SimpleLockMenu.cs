using System;
using UnityEditor;

public class SimpleLockMenu : Editor
{
    public static int m_pressed;
    public static DateTime m_timer;

    [MenuItem("Tools/Toggle Inspector Lock _`")]
    public static void ToggleInspectorLock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }
}