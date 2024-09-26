using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // public string nextSceneName = "SampleScene"; // Nome da cena a ser carregada

    void Update()
    {

        // Ao pressionar qualquer tecla o usuário sairá do menu e será direcionado para o jogo

        if (Input.anyKeyDown) 
        {
            Debug.Log("Key pressed, trying to load SampleScene");
            SceneManager.LoadScene(fast_travel.lvl1); 
        }
    }
}



