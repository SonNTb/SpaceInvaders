using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    Text scoreTextUI; //The score text UI game object

    int score;

    public int Score
    {
        get
        {
            return this.score;
        }
        set
        {
            this.score = value;
            UpdateScoreTextUI();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get the Text UI component of this gameObject
        scoreTextUI = GetComponent<Text>();
    }

    //Function to update the score text UI
    void UpdateScoreTextUI()
    {
        string scoreStr = string.Format("{0:000000}", score);
        scoreTextUI.text = scoreStr;
    }
}
