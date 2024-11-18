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
        // Check if the levelNames and buttons array lengths match
        if (levelButtons.Length != levelNames.Length)
        {
            Debug.LogError("Number of buttons and levels do not match!");
            return;
        }

        // Initialize the fade image (start fully transparent)
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0); // Transparent initially
        }

        // Set up the buttons and listeners
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i; // Cache the index to avoid closure issues
            levelButtons[i].onClick.AddListener(() =>
            {
                Debug.Log($"Button {levelNames[index]} clicked!");
                StartCoroutine(FadeAndLoadLevel(levelNames[index])); // Fade before loading
            });

            Debug.Log($"Button {levelNames[i]} set up with listener.");
        }

        // Optionally, start with a fade-in effect when the scene starts
        StartCoroutine(Fade(0f, 0f));  // Ensure the fade effect is ready if needed
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
}
