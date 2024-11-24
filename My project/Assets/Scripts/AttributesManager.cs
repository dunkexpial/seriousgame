using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    private SoundManager soundManager;
    public GameObject[] bossDrop;
    public int health;
    public int attack;
    public float damageColorDuration = 0.1f; // Duration of the red color effect
    private SpriteRenderer[] spriteRenderers;
    private Dictionary<SpriteRenderer, Color> originalColors = new Dictionary<SpriteRenderer, Color>();
    
    [Range(0f, 1f)] public float dropChance = 1f; // Chance of item drop (default 100%)

    private void Start()
    {
        // Get all SpriteRenderers, including those of child objects
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        
        // Store original colors for each SpriteRenderer
        foreach (var sr in spriteRenderers)
        {
            originalColors[sr] = sr.color;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            itemDrop();
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<AttributesManager>();
        if (atm != null)
        {
            atm.TakeDamage(attack);
        }
    }

    public void itemDrop()
    {
        // Check if the drop chance is met
        if (Random.value <= dropChance)
        {
            for (int i = 0; i < bossDrop.Length; i++)
            {
                if (bossDrop[i])
                {
                    Instantiate(bossDrop[i], transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                }
            }
        }
    }

    private IEnumerator FlashRed()
    {
        // Change color to red for each SpriteRenderer
        foreach (var sr in spriteRenderers)
        {
            sr.color = Color.red;
        }

        // Wait for the specified duration
        yield return new WaitForSeconds(damageColorDuration);

        // Revert each SpriteRenderer to its original color
        foreach (var sr in spriteRenderers)
        {
            if (originalColors.TryGetValue(sr, out Color originalColor))
            {
                sr.color = originalColor;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (soundManager != null)
        {
            soundManager.PlayarSom(GetComponent<Collider2D>(), other);
        }
    
}
}
