using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public GameObject ExplosiveGO; //Explosive Prefab
    public GameObject EnemyBulletGO; //The enemy bullet prefab
    GameObject scoreUITextGO; //Reference to the text UI game object
    private ObjectPool objectPool;

    public float moveSpeed; //Moving speed of the enemy
    public float moveSpeedMax;//The maximum speed of the enemy
    public float moveSpeedStart; //Enemy's moving speed when user press play
    public float rateOfFire; //Rate of fire
    private float timePass = 0;
    int lives; //Number of enemy's lives
    Vector2 _dir; //Direction to the Player's ship

    //private void Awake()
    //{
    //    GameObject playerShip = GameObject.Find("PlayerGO");
    //    _dir = (playerShip.transform.position - transform.position).normalized;
    //    lives = 1;
    //}
    //
    private void Start()
    {
        //Get the score UI
        //scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
        objectPool = FindObjectOfType<ObjectPool>();
    }

    private void OnEnable()
    {
        GameObject playerShip = GameObject.Find("PlayerGO");
        _dir = (playerShip.transform.position - this.transform.position).normalized;
        lives = 1;
        //Get the score UI
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }

    // Update is called once per frame
    void Update()
    {
        timePass += Time.deltaTime;
        if (timePass > rateOfFire)
        {
            timePass = 0;
            FireEnemyBullet();
        }
        //Get the enemy's current position
        Vector2 pos = this.transform.position;

        //Compute the enemy's new position
        pos += _dir * moveSpeed * Time.deltaTime;

        //Update enemy's new position
        transform.position = pos;

        //This is the bottom-left and the top-right point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); // Bottom-left
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); // Top-right

        //Remove the enemy from game if the enemy go outside the screen
        if (transform.position.x < min.x || transform.position.x > max.x || transform.position.y < min.y || transform.position.y > max.y)
        {
            //Destroy(gameObject);
            //.transform.rotation = Quaternion.identity;
            if (objectPool!=null)
            {
                objectPool.ReturnGameObject(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Detect collision of the enenmy ship with the player ship or with player's bullet
        if ((col.tag == "PlayerShipTag") || col.tag == "PlayerBulletTag")
        {
            lives--; // Subtract enemy lives

            if (lives == 0) //If the enemy dead 
            {
                PlayExplosion();
                //Add 100 points to the score
                scoreUITextGO.GetComponent<GameScore>().Score += 100;
                //Destroy this enemy ship
                //Destroy(gameObject);
                //this.transform.rotation = Quaternion.identity;
                if (objectPool != null)
                {
                    objectPool.ReturnGameObject(this.gameObject);
                }
            }
        }
    }

    //Funcion to instantiate an explosion
    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExplosiveGO);

        //Set the position of the explosion
        explosion.transform.position = transform.position;
    }

    //Function to increase enemy's moving speed
    void IncreaseMovingSpeed()
    {
        moveSpeed += 0.5f;

        if (moveSpeed == moveSpeedMax)
        {
            CancelInvoke("IncreaseMovingSpeed");
        }
    }

    //Function to start increase enemy's moving speed
    public void StartIncreaseMovingSpeed()
    {
        moveSpeed = moveSpeedStart;
        //Increase spawn rate every 30 secconds
        InvokeRepeating("IncreaseMovingSpeed", 30f, 30f);
    }

    //Function to stop increase enemy's moving speed
    public void ResetMovingSpeed()
    {
        moveSpeed = moveSpeedStart;
    }

    //Function to fire an enemy bullet
    void FireEnemyBullet()
    {
        //Get a reference to the PlayerGO
        GameObject playerShip = GameObject.Find("PlayerGO");

        //If the player still alive
        if (playerShip != null)
        {
            //Instantiate an enemy bullet
            GameObject bullet = objectPool.GetObject(EnemyBulletGO, this.gameObject.transform.GetChild(0).transform.position, Quaternion.identity);

            //Set the bullet's initial position
            //bullet.transform.position = transform.position;

            //Compute the bullet's direction toward to the Player
            Vector2 _dir = playerShip.transform.position - bullet.transform.position;

            //Set the bullet's direction
            bullet.GetComponent<EnemyBullet>().SetDirection(_dir);

        }
    }
}
