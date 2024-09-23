using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fast_travel : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

     void OnMouseDown() 
     {
        // Ao clickar vai verificar o nome do arquivo e carregar o cenário 

        if (gameObject.name == "Level1"){

            SceneManager.LoadScene("FirstLevel");

        }

        if (gameObject.name == "Level2"){

            SceneManager.LoadScene("SecondLevel");

        }

        if (gameObject.name == "Level3"){

            SceneManager.LoadScene("ThirdLevel");

        }

        if (gameObject.name == "Level4"){

            SceneManager.LoadScene("FourthLevel");

        }

        if (gameObject.name == "Level5"){

            SceneManager.LoadScene("FifthLevel");

        }
        
     }

     
}


//SceneManager.LoadScene