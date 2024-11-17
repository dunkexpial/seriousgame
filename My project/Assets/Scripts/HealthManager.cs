using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static int health = 5;
    public Image[] hearts; //Each index represents a heart in the graphical interface (UI).
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Awake() {
        health = 5;
        //After the player dies, the static health is reset to 0, but we set it back to 5 in Awake.
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

        // Update heart UI based on current health
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }
        for (int i = 0; i < health; i++)
        {
            hearts[i].sprite = fullHeart;
        }
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