using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


public class PlayerCollision : MonoBehaviour
{
    private HealthManager healthManager;

    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();  //Acess Health manager and restart the regen timer 
                                                            //It's "access", there's two Cs in that. ~JV
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if(collision.transform.tag == "Enemy")
        {
            
            HealthManager.health--;

            
            healthManager.ResetRegenTimer();

            if (HealthManager.health <= 0)
            {
                PlayerManager.GameOver = true;
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(TakeDamage());
            }
        }
    }

    IEnumerator TakeDamage()
    {
        Physics2D.IgnoreLayerCollision(6,7);
        GetComponent<Animator>().SetLayerWeight(1,1); //TakeDamage animation
        yield return new WaitForSeconds(2);
        GetComponent<Animator>().SetLayerWeight(1,0);
        Physics2D.IgnoreLayerCollision(6,7, false);

        //This function will take the layers of the player and enemy. Then After the player take damage the colilision
        //will be disabled, and guarateen 2s of invincibility to the player
    }
}