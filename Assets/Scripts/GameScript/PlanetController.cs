using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public GameObject[] Planets; //Array of PlanetGO prefabs

    //Queue to hold the planets
    Queue<GameObject> avaiblePlanets = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //Add the planets to the Queue (Enqueue them)
        avaiblePlanets.Enqueue(Planets[0]);
        avaiblePlanets.Enqueue(Planets[1]);
        avaiblePlanets.Enqueue(Planets[2]);

        //Call the MovePlanetDown function every 20 seconds
        InvokeRepeating("MovePlanetDown", 0, 20f);
    }

    //Function to dequeue the planet, and set its isMoving flag to true
    //So that planet starts scrolling down the screen
    void MovePlanetDown()
    {
        EnqueuePlanet();
        //If the queue is empty, then return 
        if (avaiblePlanets.Count == 0)
        {
            return;
        }

        //Get a planet from the queue
        GameObject aPlanet = avaiblePlanets.Dequeue();

        //Set the planet isMoving to true
        aPlanet.GetComponent<Planet>().isMoving = true;
    }

    //Function to Enqueue planets that are below the screen and are not moving
    void EnqueuePlanet()
    {
        foreach (GameObject aPlanet in Planets)
        {
            //If the planet is below the screen, and the planet is not moving
            if ((aPlanet.transform.position.y < 0) && (!aPlanet.GetComponent<Planet>().isMoving))
            {
                //reset the planet position
                aPlanet.GetComponent<Planet>().ResetPostion();

                //Enqueue the planet
                avaiblePlanets.Enqueue(aPlanet);
            }
        }
    }
}
