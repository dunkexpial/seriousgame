using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPlayerPrefsButton : MonoBehaviour
{
    public LevelSelector levelSelector; // Reference to the LevelSelector script

    // Method to reset PlayerPrefs
    public void ResetPlayerPrefs()
    {
        // Delete all PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Log to confirm PlayerPrefs have been reset
        Debug.Log("PlayerPrefs have been reset.");

        // Manually reset the unlocked level to 1 (or desired starting level)
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.Save();

        // Refresh the level buttons to reflect the reset
        if (levelSelector != null)
        {
            levelSelector.RefreshLevelButtons();  // Update button states immediately
        }
        else
        {
            Debug.LogWarning("LevelSelector reference not set in ResetPlayerPrefsButton.");
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
