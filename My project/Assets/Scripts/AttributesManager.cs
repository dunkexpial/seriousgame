using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    public GameObject[] bossDrop;
    public int health;
    public int attack;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            itemDrop();
            Destroy(gameObject);
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
}