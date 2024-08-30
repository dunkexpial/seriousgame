using UnityEngine;
using UnityEngine.SceneManagement; 

public class Exit : MonoBehaviour
{
 void OnMouseDown() 
     {
        if (gameObject.name == "Exit"){

            SceneManager.LoadScene("FastTravel");

        }
        
     }
}
