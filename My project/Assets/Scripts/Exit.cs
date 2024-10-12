using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnHover : MonoBehaviour
{

    // private void OnTriggerEnter(Collider other)
    // {
    //     // Verifica se o objeto que entrou tem a tag "Player"
    //     if (other.CompareTag("Player"))
    //     {
    //         Debug.Log("Player entrou no trigger de transição de cena.");
    
    //         SceneManager.LoadScene("SecondLevel");
    //     }
    // }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entrou no trigger de transição de cena.");
    
            SceneManager.LoadScene(fast_travel.lvl2);
        }
    }
}