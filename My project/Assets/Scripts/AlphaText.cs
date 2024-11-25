using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelSelector : MonoBehaviour
{
    public string[] levelNames; // Array to store level names
    public Button[] levelButtons; // Array to store buttons corresponding to levels
    public Image fadeImage; // Image to use for fade effect
    public float fadeDuration = 1f; // Duration of the fade

    void Start()
    {
        PauseMenu.isPaused = false;
        if (levelButtons.Length != levelNames.Length)
        {
            Debug.LogError("Number of buttons and levels do not match!");
            return;
        }

        // Initialize fade image (start fully transparent)
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }
        PlayerManager.reachedBossArea = false;

        // Retrieve the highest unlocked level from PlayerPrefs
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // Set up buttons and their states
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i; // Cache the index for listener
            bool isUnlocked = (i + 1) <= unlockedLevel;

            levelButtons[i].interactable = isUnlocked; // Lock or unlock the button

            // Add listener for button click
            if (isUnlocked)
            {
                levelButtons[i].onClick.AddListener(() =>
                {
                    Debug.Log($"Button {levelNames[index]} clicked!");
                    StartCoroutine(FadeAndLoadLevel(levelNames[index]));
                });
            }
            else
            {
                Debug.Log($"Button {levelNames[index]} is locked.");
            }
        }

        // Start with fade-in effect
        StartCoroutine(Fade(0f, 0f));
    }

    public void UnlockNextLevel(int currentLevel)
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (currentLevel >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save(); // Save immediately to persist data
            Debug.Log($"Unlocked level {currentLevel + 1}");
        }
    }


    private IEnumerator FadeAndLoadLevel(string levelName)
    {
        // Ensure the fade image is visible for the fade
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true); // Ensure fadeImage is active
        }

        // Fade to black (opaque)
        yield return StartCoroutine(Fade(0f, 1f));

        // Wait for the fade duration before loading the level
        yield return new WaitForSeconds(fadeDuration/2);

        // After fade-out, load the level
        LoadLevel(levelName);
    }


    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (fadeImage == null) yield break;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }

        // Ensure the final alpha value is set
        fadeImage.color = new Color(0, 0, 0, endAlpha);
    }

    void LoadLevel(string levelName)
    {
        Debug.Log($"Loading level: {levelName}");
        if (Application.CanStreamedLevelBeLoaded(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogError($"Scene '{levelName}' cannot be loaded. Ensure it's added to Build Settings!");
        }
    }
    public void RefreshLevelButtons()
    {
        // Retrieve the highest unlocked level from PlayerPrefs
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // Update button states based on the unlocked level
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i; // Cache the index for listener
            bool isUnlocked = (i + 1) <= unlockedLevel;

            levelButtons[i].interactable = isUnlocked; // Lock or unlock the button
            levelButtons[i].onClick.RemoveAllListeners(); // Remove previous listeners

            if (isUnlocked)
            {
                levelButtons[i].onClick.AddListener(() =>
                {
                    Debug.Log($"Button {levelNames[index]} clicked!");
                    StartCoroutine(FadeAndLoadLevel(levelNames[index]));
                });
            }
            else
            {
                Debug.Log($"Button {levelNames[index]} is locked.");
            }
        }
    }
}
