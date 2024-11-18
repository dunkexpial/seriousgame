using UnityEditor;
using UnityEngine;

public class ResetPlayerPrefsEditor : EditorWindow
{
    [MenuItem("Tools/Reset PlayerPrefs")]
    public static void ResetPlayerPrefs()
    {
        // Delete all PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Log to confirm PlayerPrefs have been reset
        Debug.Log("PlayerPrefs have been reset.");
    }
}
