using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnHover : MonoBehaviour
{
    public static SceneChangeOnHover instance; // Portal Singleton instance
    public Vector3 nextSpawnPoint = new Vector3(0, 0, 0);
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Sets the instance to this object
        }
        else
        {
            Destroy(gameObject); // Makes it so that only one little shit exists one instance of the portal
        }

        gameObject.SetActive(false); 
    }
    public void ActivePortal()
    {
        gameObject.SetActive(true); 
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the scene transition trigger.");

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                //Save spawn point for next level
                PlayerPrefs.SetFloat("SpawnX", nextSpawnPoint.x);
                PlayerPrefs.SetFloat("SpawnY", nextSpawnPoint.y);
                PlayerPrefs.SetFloat("SpawnZ", nextSpawnPoint.z);

                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(currentSceneIndex + 1);
            }  
        } 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //Finds the player and positions it at the saved coordinate
            float x = PlayerPrefs.GetFloat("SpawnX");
            float y = PlayerPrefs.GetFloat("SpawnY");
            float z = PlayerPrefs.GetFloat("SpawnZ");
            player.transform.position = new Vector3(x, y, z);
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
