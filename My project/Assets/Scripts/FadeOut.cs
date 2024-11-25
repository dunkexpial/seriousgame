using System.Collections;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 3f; // Duration of the fade effect
    public bool shouldWaitBeforeFade = false; // Flag to enable/disable wait before fade
    public float waitBeforeFade = 0f; // Time to wait before starting the fade, if enabled

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            StartCoroutine(FadeOutRoutine());
        }
        else
        {
            Debug.LogWarning("FadeOut script requires a SpriteRenderer on the GameObject.");
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        if (shouldWaitBeforeFade && waitBeforeFade > 0f)
        {
            yield return new WaitForSeconds(waitBeforeFade);
        }

        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure the marker is completely invisible and destroy it
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        Destroy(gameObject);
    }
}