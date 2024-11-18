using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletion : MonoBehaviour
{
    void Start()
    {
        // Get the index of the current level from SceneManager
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        UnlockCurrentLevel(currentLevel);
    }

    private void UnlockCurrentLevel(int currentLevel)
    {
        // Retrieve the highest unlocked level from PlayerPrefs
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // Unlock the current level if it's higher than the unlocked level
        if (currentLevel > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel);  // Unlock the current level
            PlayerPrefs.Save(); // Save to persist data
            Debug.Log($"Unlocked level {currentLevel}");
        }
    }
}
