using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public static string lvl1 = "FirstLevel";
    public static string lvl2 = "SecondLevel";
    // public string nextSceneName = "SampleScene"; // Nome da cena a ser carregada
    void Update()
    {

        // Ao pressionar qualquer tecla o usuário sairá do menu e será direcionado para o jogo

        if (Input.anyKeyDown)
        {
            Debug.Log("Key pressed, trying to load SampleScene");
            SceneManager.LoadScene(lvl2); 
        }
    }
}



