using UnityEngine;

public class ActivateSwap : MonoBehaviour
{
    [Header("Objects to Toggle")]
    public GameObject objectA;
    public GameObject objectB;

    public void Toggle()
    {
        if (objectA == null || objectB == null) return;

        bool isAActive = objectA.activeSelf;
    
        objectA.SetActive(!isAActive);
        objectB.SetActive(isAActive);
    }
}
