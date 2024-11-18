using UnityEngine;

public class ResetPlayerPrefsButton : MonoBehaviour
{
    // Method to reset PlayerPrefs
    public void ResetPlayerPrefs()
    {
        // Delete all PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Log to confirm PlayerPrefs have been reset
        Debug.Log("PlayerPrefs have been reset.");
    }
}
