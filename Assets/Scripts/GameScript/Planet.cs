using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float speed; //Speed of the planet
    public bool isMoving; //Flag to make the planet scroll down the screen

    Vector2 min; //Bottom-left point of the screen
    Vector2 max; //Top-right point of the screen

    private void Awake()
    {
        isMoving = false;
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //Add the planet half height to max y
        max.y = max.y + GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

        //Subtract the planet sprite half height to min y
        min.y = min.y - GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            return;
        }

        //Get the current position of the planet
        Vector2 position = transform.position;

        //Compute the planet's new position
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);

        //Update the planet's position
        transform.position = position;

        //If the planet gets to minimum y position. then stop moving the planet
        if (transform.position.y < min.y)
        {
            isMoving = false;
        }
    }

    //Function to reset the planet's position
    public void ResetPostion()
    {
        //reset the position of the planet to random x, max y
        transform.position = new Vector2(Random.Range(min.x, max.x), max.y);
    }
}
