using System.Collections;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 3f; // Duration of the fade effect

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
