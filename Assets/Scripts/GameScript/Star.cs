using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float speed; //The speed of the star

    // Update is called once per frame
    void Update()
    {
        //Get the current position of the star
        Vector2 position = transform.position;

        //Compute the star's new position
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);

        //Update the star's position
        transform.position = position;

        //This is the bottom-left and the top-right point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); // Bottom-left
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); // Top-right

        //If the star goes outside the screen bottom
        //Then position the star on the top edge of the screen
        //And randomly between left anh right side of the screen
        if (transform.position.y < min.y)
        {
            transform.position = new Vector2(Random.Range(min.x, max.x), max.y);
        }

    }
}
