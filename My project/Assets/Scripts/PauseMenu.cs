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
    public static bool isPaused;

    [SerializeField] private AudioSource backgroundMusic; // Música de fundo do nível
    private AudioSource currentMusic; // Música atualmente em reprodução (padrão ou especial)

    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        currentMusic = backgroundMusic; // Define a música inicial como a música de fundo
        if (currentMusic == null)
        {
            Debug.LogError("AudioSource da música de fundo não está vinculado no Inspector!");
        }
    }

    void Update()
    {
        iventorySlots.SetActive(!isPaused);
        healthBar.SetActive(!isPaused);

        if (PlayerManager.GameOver) return;

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

        // Pausa a música atual
        if (currentMusic != null && currentMusic.isPlaying)
        {
            currentMusic.Pause();
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Retoma a música atual
        if (currentMusic != null)
        {
            currentMusic.UnPause();
        }
    }

    // Altera a música atualmente tocando
    public void SetCurrentMusic(AudioSource newMusic)
    {
        if (currentMusic != null && currentMusic.isPlaying)
        {
            currentMusic.Stop(); // Para a música atual
        }

        currentMusic = newMusic; // Define a nova música
        if (currentMusic != null)
        {
            currentMusic.Play(); // Toca a nova música
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
        Physics2D.IgnoreLayerCollision(6, 7, false);

        pauseMenu.SetActive(false);
        iventorySlots.SetActive(false);
        healthBar.SetActive(false);
        isPaused = false;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
