using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static int health = 5;
    public Image[] hearts; //Each index represent a heart in the graphical interface (UI).
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public float regenDelay = 5f; //it takes 5s to start regenerating
    private bool isRegenerating = false;
    private Coroutine regenCoroutine; 

     void Awake() {
        health = 5;
        //After the player die the static of the health will be 0
        //Awake function solve this issue, and the player have all 5 hearts back
    }
    

    void Update()
    {
        if (Time.timeScale == 0)
        {
            HideHearts(); 
            return; 
        }
        else
        {
            ShowHearts(); 
        }

        //Basically the for loop change the index of the healt bar into different sprites [full ,empty]
        //Every time it takes damage or heal
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }
        for (int i = 0; i < health; i++)
        {
            hearts[i].sprite = fullHeart;
        }

        if (health < 5 && !isRegenerating)
        {
            regenCoroutine = StartCoroutine(RegenHealth());
        }
        
    }

    IEnumerator RegenHealth()
    {
        isRegenerating = true;
        yield return new WaitForSeconds(regenDelay); //Wait 5 seconds to start regenarating

        if (health < 5) 
        {
            health++;
        }

        isRegenerating = false; //Allow to regenate again. This mf took me 1 hour to discover why the player wasn't regenerating again. >:(
    }

    public void ResetRegenTimer()
    {
        //This function will reset the timer if the player takes damage within the 5s required to regenerate 
        //It was worth it? NO!! But i can and i did it 
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine); //Stop the current 5f 
        }
        isRegenerating = false; //Timer restart to 5f regenerate again
    }

    private void HideHearts()
    {
        foreach (Image heart in hearts)
        {
            heart.enabled = false;
        }
    }

    private void ShowHearts()
    {
        foreach (Image heart in hearts)
        {
            heart.enabled = true;
        }

    }
}


