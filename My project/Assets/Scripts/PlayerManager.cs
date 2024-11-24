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
    public static bool reachedBossArea; // Tracks if the player reached the boss area

    private void Awake()
    {
        GameOver = false;
        Time.timeScale = 1f;

        if (reachedBossArea)
        {
            TeleportToBossSpawnPoint();
        }
    }

    void Update()
    {
        // Display a message you died and a button to restart the level
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

    private void TeleportToBossSpawnPoint()
    {
        GameObject bossSpawnPoint = GameObject.FindGameObjectWithTag("BossRespawnPoint");
        if (bossSpawnPoint != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = bossSpawnPoint.transform.position;
            }
        }
    }
}
