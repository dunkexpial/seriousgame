using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnHover : MonoBehaviour
{
    public static SceneChangeOnHover instance; // Portal Singleton instance

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Sets the instance to this object
        }
        else
        {
            Destroy(gameObject); // Makes sure only one instance of the portal exists
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
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
        } 
    }
}
