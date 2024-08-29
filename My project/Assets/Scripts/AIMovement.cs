using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public float speed;
    public float radius;
    private GameObject player;
    private float distance;

    void Start()
    {
        // Encontra o GameObject com a tag "PlayerTag"
        player = GameObject.FindGameObjectWithTag("PlayerTag");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            // Loop até encontrar o player
            player = GameObject.FindGameObjectWithTag("PlayerTag");
            if (player == null) return;
        }

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        if (distance < radius)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}
