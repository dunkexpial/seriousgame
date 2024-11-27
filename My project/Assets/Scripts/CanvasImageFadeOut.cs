using UnityEngine;
using UnityEngine.UI;
using System.Collections;   

public class FadeOutImage : MonoBehaviour
{
    public float fadeDuration = 1f; // Duration of the fade-out in seconds.
    private Image imageComponent;
    private bool isFading = false;

    private void Start()
    {
        // Start fade-out immediately when the script initializes
        imageComponent = GetComponent<Image>();

        if (imageComponent == null)
        {
            Debug.LogError("No Image component found on the GameObject. Attach this script to a GameObject with an Image component.");
            return;
        }

        StartFadeOut();
    }


    public void StartFadeOut()
    {
        if (!isFading && imageComponent != null)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0f;
        Color originalColor = imageComponent.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            imageComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure the image is fully transparent at the end of the fade.
        imageComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        isFading = false;
    }
}
