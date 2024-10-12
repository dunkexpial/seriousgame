using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerManager : MonoBehaviour
{
    
    public static bool GameOver;
    public GameObject GameOverScreen;
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
        } 
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(fast_travel.lvl1);

    }
}
