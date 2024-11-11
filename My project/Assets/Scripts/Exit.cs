using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnHover : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entrou no trigger de transição de cena.");

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
            }

        }
    }
}
