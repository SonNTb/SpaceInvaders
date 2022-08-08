using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletControl : MonoBehaviour
{
    private void Update()
    {
        // Get the bullet's current position 
        Vector2 pos = transform.position;

        //This is the bottom-left and the top-right point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); // Bottom-left
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); // Top-right

        //Remove the bullet from game if the bullet go outside the screen
        if (pos.x < min.x || pos.x > max.x || pos.y < min.y || pos.y > max.y) 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Detect colliion of the player bullet with an enemy ship
        if (collision.tag == "EnemyShipTag")
        {
            //Destroy this player bullet
            Destroy(gameObject);
        }
    }
}
