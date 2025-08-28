using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public float fadeDuration = 1f; // Time to fade in/out
    public float waitTime = 1f;
    private Image fadeImage;
    private TextMeshProUGUI fadeText;
    private bool isFading;  

    private void Awake()
    {
        // Try to get the Image component
        fadeImage = GetComponent<Image>();

        // Try to get the TextMeshProUGUI component
        fadeText = GetComponent<TextMeshProUGUI>();

        if (fadeImage == null && fadeText == null)
        {
            Debug.LogError("SceneFader: No Image or TextMeshProUGUI component found!");
        }

        // Start fully visible (black for Image, white for TextMeshPro)
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 1); // Black color with full opacity
        }
        else if (fadeText != null)
        {
            fadeText.color = new Color(1, 1, 1, 1); // White text with full opacity
        }

        isFading = true;
    }

    private void Start()
    {
        // Start the fade-in effect when the scene loads
        FadeIn();
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(1f, 0f)); // Fade from black/white to transparent
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(0f, 1f)); // Fade from transparent to black/white
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        yield return new WaitForSeconds(waitTime);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            // If we are fading an Image
            if (fadeImage != null)
            {
                fadeImage.color = new Color(0, 0, 0, newAlpha);
            }

            // If we are fading TextMeshProUGUI
            if (fadeText != null)
            {
                fadeText.color = new Color(1, 1, 1, newAlpha); // Assuming white text color
            }

            yield return null;
        }

        // Ensure the final alpha value is set
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, endAlpha);
        }

        if (fadeText != null)
        {
            fadeText.color = new Color(1, 1, 1, endAlpha);
        }

        isFading = false;

        // Destroy the game object after the fade effect is complete
        yield return new WaitForSeconds(waitTime);
        // Also destroy the parent object
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
