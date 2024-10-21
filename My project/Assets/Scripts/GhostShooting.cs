using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShooting : MonoBehaviour
{
    public GameObject ghostProjectile;
    public Transform ghostProjPos;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 0.5)
        {
            timer = 0;
            shoot();
        }
    }

    void shoot()
    {
        Instantiate(ghostProjectile, ghostProjPos.position, Quaternion.identity);
    }
}
