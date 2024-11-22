using UnityEngine;

public class UnlockAllLevelsButton : MonoBehaviour
{
    public LevelSelector levelSelector; // Reference to the LevelSelector script

    // Method to unlock all levels
    public void UnlockAllLevels()
    {
        // Unlock all levels (set UnlockedLevel to 5)
        PlayerPrefs.SetInt("UnlockedLevel", 5);
        PlayerPrefs.Save();

        // Log to confirm all levels have been unlocked
        Debug.Log("All levels unlocked!");

        // Refresh the level buttons to reflect the new unlocked state
        if (levelSelector != null)
        {
            levelSelector.RefreshLevelButtons();  // Immediately update buttons
        }
        else
        {
            Debug.LogWarning("LevelSelector reference not set in UnlockAllLevelsButton.");
        }
    }
}
