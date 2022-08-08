using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    Text timeUI;//Reference to the time counter UI Text

    float startTime;//The time when the user clicks on play
    float elapsedTime; //The ellapsed time after the user clicks on play
    bool startEncounter;//Flag to start encounter

    int minutes;
    int seconds;

    // Start is called before the first frame update
    void Start()
    {
        startEncounter = false;

        //Get the Text UI component from this gameObject
        timeUI = GetComponent<Text>();
    }

    //Function to start the time counter
    public void StartTimeEncounter()
    {
        startTime = Time.time;
        startEncounter = true;
    }

    //Function to stop the time encounter
    public void StopTimeEncounter()
    {
        startEncounter = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Start to count the time
        if (startEncounter)
        {
            //Compute the elapsed time
            elapsedTime = Time.time - startTime;

            minutes = (int)elapsedTime / 60; //Get the minutes
            seconds = (int)elapsedTime % 60; //Get the seconds

            //Update the time counter UI Text
            timeUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
