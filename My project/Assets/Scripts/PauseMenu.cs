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
    public static string mm = "MainMenu";
    public GameObject pauseMenu;
    public GameObject iventorySlots;
    public GameObject healthBar;
    public bool isPaused;
    public Vector3 respawnCoordinates;

    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
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
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
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
        Physics2D.IgnoreLayerCollision(6,7, false); //Now the player Actually take damage after restart

        pauseMenu.SetActive(false);
        iventorySlots.SetActive(false);
        healthBar.SetActive(false);
        isPaused = false;

        HealthManager.health = 5;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.sceneLoaded += OnSceneReloaded;
        SceneManager.LoadScene(currentScene.name);

    }

    private void OnSceneReloaded(Scene scene, LoadSceneMode mode)
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = respawnCoordinates;
        }
        SceneManager.sceneLoaded -= OnSceneReloaded;
    }


}