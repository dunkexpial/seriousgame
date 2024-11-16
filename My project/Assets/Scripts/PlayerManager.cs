using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerManager : MonoBehaviour
{
    public static bool GameOver;
    public GameObject GameOverScreen;
    public GameObject iventorySlots;
    public GameObject player;
    public GameObject virtualCamera;
    public Vector3 respawnCoordinates;

    private void Awake()
    {
        GameOver = false; 
        Time.timeScale = 1f;  

        if (player != null)
        {
            DontDestroyOnLoad(player);
        }

        if (virtualCamera != null)
        {
            DontDestroyOnLoad(virtualCamera);
        }
    }
    void Update()
    {
        //Display a message you died and a button to restar the level
        if (GameOver)
        {
            GameOverScreen.SetActive(true);
            iventorySlots.SetActive(false);
        } 
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.sceneLoaded += OnSceneReloaded;
        SceneManager.LoadScene(currentScene.name);

        StartCoroutine(ResetGameState());
    }

    private IEnumerator ResetGameState()
    {
        yield return new WaitForEndOfFrame();

        GameOver = false;

        ResetObjectState(GameOverScreen, false);
        ResetObjectState(iventorySlots, true);
        ResetObjectState(player, true);
        ResetObjectState(virtualCamera, true);

        HealthManager.health = 5;
    }
    private void ResetObjectState(GameObject obj, bool state)
    {
        if (obj != null){obj.SetActive(state);}
    }

    private void OnSceneReloaded(Scene scene, LoadSceneMode mode)
    {

        if (player != null)
        {
            player.transform.position = respawnCoordinates;
        }

        SceneManager.sceneLoaded -= OnSceneReloaded;
    }
}


