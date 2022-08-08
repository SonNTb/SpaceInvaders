using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyGO; //The EnemyGO prefabs
    public Transform[] spawnPoints; //List of points where enemies are spawned

    public float spawnRate; //The rate to spawn enemy after user clicks on play
    float startSpawnRate; //The rate to spawn enemy when the user clicks on play
    int randomSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        startSpawnRate = spawnRate;
    }

    //Function to spawn an enemy
    void SpawnEnemy()
    {
        //Get random spawn point
        randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        Vector2 spawnPos = GameObject.Find("PlayerGO").transform.position;

        //Find the direction to the Player's current posion
        GameObject playerShip = GameObject.Find("PlayerGO");
        Vector2 dir = playerShip.transform.position - spawnPoints[randomSpawnPoint].position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Instantiate an enemy
        Instantiate(EnemyGO, spawnPoints[randomSpawnPoint].position, rotation);

        //Schedule when to spawn next enemy
        ScheduleNextEnemySpawn();
    }

    //Function to schedule time to spawn the next enemy
    void ScheduleNextEnemySpawn()
    {
        float spawnInNSeconds;

        if (spawnRate > 1f)
        {
            //Pick a number betweeen 1 and spawnRate
            spawnInNSeconds = Random.Range(1f, spawnRate);
        }
        else
        {
            spawnInNSeconds = 1f;
        }

        Invoke("SpawnEnemy", spawnInNSeconds);
    }

    //Function to increase the difficulty of the game
    void IncreaseSpawnRate()
    {
        if (spawnRate > 1f)
        {
            spawnRate--;
        }

        if (spawnRate == 1f)
        {
            CancelInvoke("IncreaseSpawnRate");
        }
    }

    //Function to start enemy spawner
    public void ScheduleEnemySpawner()
    {
        //reset the spawn rate
        spawnRate = startSpawnRate;

        //Start to spawn enemies
        Invoke("SpawnEnemy", spawnRate);

        //Increase spawn rate every 30 secconds
        InvokeRepeating("IncreaseSpawnRate", 0f, 30f);
    }

    //Function to stop enemy spawner
    public void UnscheduleEnemyspawner()
    {
        CancelInvoke("SpawnEnemy");
        CancelInvoke("IncreaseSpawnRate");
    }
}
