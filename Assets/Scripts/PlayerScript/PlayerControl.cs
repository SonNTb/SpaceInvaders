using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;

    public float fireForce;     //Fire Force
    public float fireRate;      //Rate of fire
    private float lastShootTime = 0;

    public GameObject PlayerBulletGO; //Bullet Prefab
    public Transform firePoint1;    //Shooting postion 1
    public Transform firePoint2;    //Shooting postion 2
    public GameObject ExplosionGO; //Explosive Prefab
    public GameObject GameManagerGO; //reference to our game manager
    ObjectPool objectPool;

    AudioSource audioData;

    public Text LivesUIText; //Reference to the lives UItext
    const int MaxLives = 3;//maximum player lives
    int lives;// current player lives

    public void Init()
    {
        lives = MaxLives;

        //Update the lives UI text
        LivesUIText.text = lives.ToString();

        //Reset the player position to the center of the screen
        transform.position = new Vector2(0, 0);

        //Set this player game object to active
        gameObject.SetActive(true);
        audioData = GetComponent<AudioSource>();
        objectPool = FindObjectOfType<ObjectPool>();

    }

    // Update is called once per frame
    void Update()
    {
        lastShootTime += Time.deltaTime;
        Movevement();
        Rotation();
        if (Input.GetMouseButton(0) && lastShootTime > fireRate)
        {
            //play the laser sound effect
            audioData.Play();
            lastShootTime = 0;
            Fire();
        }
    }

    //Function handle the movement of the Player when input W,S,A,D
    void Movevement()
    {
        float x = Input.GetAxisRaw("Horizontal"); // the value will be -1, 0, 1 for left, no input and right
        float y = Input.GetAxisRaw("Vertical"); // the value will be -1, 0, 1 for down, no input and up

        Vector2 direction = new Vector2(x, y).normalized;

        //This is the bottom-left and the top-right point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); // Bottom-left
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); // Top-right

        max.x = max.x - 0.225f; // subtract the player sprite half width
        min.x = min.x + 0.225f; // add the player sprite half width

        max.y = max.y - 0.285f; // subtract the player sprite half height
        min.y = min.y + 0.285f; // add the player sprite half height

        // Get the player's current position 
        Vector2 pos = transform.position;

        // Calculate the new position
        pos += direction * moveSpeed * Time.deltaTime;

        //Not move outside the screen
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    //Make Player always aim mouse's current position
    void Rotation()
    {
        //Get the direction and angle to rotate
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90 ;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    // Fire bullet Object when left click
    void Fire()
    {
        //Init the bullet
        //GameObject bullet01 = Instantiate(PlayerBulletGO, firePoint1.position, firePoint1.rotation);
        //GameObject bullet02 = Instantiate(PlayerBulletGO, firePoint2.position, firePoint2.rotation);
        GameObject bullet01 = objectPool.GetObject(PlayerBulletGO, firePoint1.position, firePoint1.rotation);
        //bullet01.transform.position = firePoint1.position;
        //bullet01.transform.rotation = firePoint1.rotation;

        GameObject bullet02 = objectPool.GetObject(PlayerBulletGO, firePoint2.position, firePoint2.rotation);
        //bullet02.transform.position = firePoint2.position;
        //bullet02.transform.rotation = firePoint2.rotation;

        bullet01.GetComponent<Rigidbody2D>().AddForce(firePoint1.up * fireForce, ForceMode2D.Impulse);
        bullet02.GetComponent<Rigidbody2D>().AddForce(firePoint2.up * fireForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Detect collision of the player ship with an enemy ship or with an enenmy bullet
        if ((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag"))
        {
            PlayExplosion();
            lives--; //Subtract one live
            LivesUIText.text = lives.ToString();//Update the lives UI text

            if (lives == 0)//If the player dead
            {
                //Change game manager state to game over state
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);

                //Destroy(gameObject); //Destroy the player's ship

                //Hide the player's ship
                gameObject.SetActive(false);
            }
        }
    }

    //Function to instantiate an explosion
    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExplosionGO);

        //set the position of the explosion
        explosion.transform.position = transform.position;
    }
}
