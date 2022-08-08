using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnenyGun : MonoBehaviour
{
    public GameObject EnemyBulletGO; //The enemy bullet prefab


    // Start is called before the first frame update
    void Start()
    {
        // Fire an enemy bullet after 1 second
        Invoke("FireEnemyBullet", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            GameObject bullet = (GameObject)Instantiate(EnemyBulletGO);

            //Set the bullet's initial position
            bullet.transform.position = transform.position;

            //Compute the bullet's direction toward to the Player
            Vector2 _dir = playerShip.transform.position - bullet.transform.position;

            //Set the bullet's direction
            bullet.GetComponent<EnemyBullet>().SetDirection(_dir);

        }
    }
}
