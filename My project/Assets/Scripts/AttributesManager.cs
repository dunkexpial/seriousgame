using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    public GameObject[] bossDrop;
    public int health;
    public int attack;
    public float damageColorDuration = 0.1f; // Duration of the red color effect
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
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
        if(atm != null)
        {
            atm.TakeDamage(attack);
        }
    }

    private void itemDrop()
    {
        for (int i = 0; i < bossDrop.Length; i++)
        {
            if (bossDrop[i])
            {
                Instantiate(bossDrop[i], transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            }
        }
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            // Change color to red
            spriteRenderer.color = Color.red;

            // Wait for the specified duration
            yield return new WaitForSeconds(damageColorDuration);

            // Revert to original color
            spriteRenderer.color = originalColor;
        }
    }
}
