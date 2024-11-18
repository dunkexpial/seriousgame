using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerManager : MonoBehaviour
{
    public static bool GameOver;
    public GameObject GameOverScreen;
    public GameObject iventorySlots;
    public GameObject healthBar;

    private void Awake()
    {
        GameOver = false; 
        Time.timeScale = 1f;  
    }
    void Update()
    {
        //Display a message you died and a button to restar the level
        if (GameOver)
        {
            GameOverScreen.SetActive(true); 
            iventorySlots.SetActive(false);
            healthBar.SetActive(false);
        } 
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

    }
}