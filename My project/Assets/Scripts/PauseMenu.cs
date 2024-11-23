using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static string mm = "MainMenu";
    public GameObject pauseMenu;
    public GameObject iventorySlots;
    public GameObject healthBar;
    public bool isPaused;

    [SerializeField] private AudioSource backgroundMusic; // Vincule o AudioSource da música pelo Inspector

    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        if (backgroundMusic == null)
        {
            Debug.LogError("AudioSource da música de fundo não está vinculado no Inspector!");
        }
    }

    void Update()
    {
        iventorySlots.SetActive(!isPaused);
        healthBar.SetActive(!isPaused);

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

        // Pausa a música
        if (backgroundMusic != null && backgroundMusic.isPlaying)
        {
            backgroundMusic.Pause();
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Retoma a música de onde parou
        if (backgroundMusic != null)
        {
            backgroundMusic.UnPause();
        }
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene(mm);
    }

    public void RestartGame()
    {
        Debug.Log("Restarting level1");
        Time.timeScale = 1f;
        Physics2D.IgnoreLayerCollision(6, 7, false); //Now the player Actually take damage after restart

        pauseMenu.SetActive(false);
        iventorySlots.SetActive(false);
        healthBar.SetActive(false);
        isPaused = false;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
