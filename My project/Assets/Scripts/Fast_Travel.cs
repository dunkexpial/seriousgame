using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fast_travel : MonoBehaviour
{

    public static string lvl1 = "FirstLevel";
    public static string lvl2 = "SecondLevel";
    public static string lvl3 = "ThirdLevel";
    public static string lvl4 = "FourthLevel";
    public static string lvl5 = "FifthLevel";

    public static string mm = "MainMenu";


   

     void OnMouseDown() 
     {
        // Ao clickar vai verificar o nome do arquivo e carregar o cenário 

        if (gameObject.name == "Level1"){

            SceneManager.LoadScene(lvl1);

        }

        if (gameObject.name == "Level2"){

            SceneManager.LoadScene(lvl2);

        }

        if (gameObject.name == "Level3"){

            SceneManager.LoadScene(lvl3);

        }

        if (gameObject.name == "Level4"){

            SceneManager.LoadScene(lvl4);

        }

        if (gameObject.name == "Level5"){

            SceneManager.LoadScene(lvl5);

        }
        
     }

     
}


//SceneManager.LoadScene