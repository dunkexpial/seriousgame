using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public GameObject iventorySlots;
    public bool isPaused;
    public AudioSource backgroundMusic;  // Música de fundo

    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {

        if (Time.timeScale == 0)
        {
            iventorySlots.SetActive(false);
        }
        else
        {
            iventorySlots.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
         if (backgroundMusic != null)
        {
            backgroundMusic.Pause();  // Pausa a música
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        if (backgroundMusic != null)
        {
            backgroundMusic.UnPause();  // Retoma a música
        }
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene(fast_travel.mm);
    }

    public void RestartGame()
    {
        Debug.Log("Restarting level1");
        Time.timeScale = 1f;
        Physics2D.IgnoreLayerCollision(6,7, false); //Now the player Actually atke damage after restart
        SceneManager.LoadScene(fast_travel.lvl1);
    }

}