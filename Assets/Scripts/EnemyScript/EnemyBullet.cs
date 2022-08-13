using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed; //Move speed of the bullet
    public float speedMax; //Maximum speed of the bullet
    public float speedStart; //Move speed when user press play
    Vector2 _dir; //The dirrection of the bullet
    bool isReady; //To know when thee bullet direction is set
    private ObjectPool objectPool;

    // Set default value in Awake function
    private void Awake()
    {
        objectPool = FindObjectOfType<ObjectPool>();
        isReady = false;
    }

    //Function to set the bullet's direction
    public void SetDirection(Vector2 direction)
    {
        //Set the direction normalized, to get an unit vector
        _dir = direction.normalized;
        isReady = true; //set flag to true
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            //Get the bullet's current position
            Vector2 pos = transform.position;

            //Compute the bullet's position
            pos += _dir * speed * Time.deltaTime;

            //Update the bullet's position
            transform.position = pos;

            //Find the screen limits (4 edges of the screen)
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); // Bottom-left
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); // Top-right

            //Remove the bullet from game if the the bullet go outside the screen
            if (transform.position.x < min.x || transform.position.x > max.x || transform.position.y < min.y || transform.position.y > max.y)
            {
                //Destroy(gameObject);
                isReady = false;
                objectPool.ReturnGameObject(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Detect collision of an enemy's bullet with the player ship
        if (collision.tag == "PlayerShipTag")
        {
            //Destroy this enemy's bullet
            //Destroy(gameObject);
            isReady = false;
            objectPool.ReturnGameObject(this.gameObject);
        }
    }

    //Function to increase enemy's bullet moving speed
    void IncreaseMovingSpeed()
    {
        speed++; //Increase enemy's bullet moving speed

        if (speed == speedMax) //Stop when the moving speed reachs maximum
        {
            CancelInvoke("IncreaseMovingSpeed");
        }
    }

    //Function to start increase enemy's bullet moving speed
    public void StartIncreaseMovingSpeed()
    {
        speed = speedStart;

        InvokeRepeating("IncreaseMovingSpeed",30f, 30f);
    }

    //Function to reset enemy's bullet moving speed to the original
    public void ResetMovingSpeed()
    {
        speed = speedStart;
    }


}
