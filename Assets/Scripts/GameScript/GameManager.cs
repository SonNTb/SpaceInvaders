using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Referrence to our game objects
    public GameObject playButton; //reference to the Play Button
    public GameObject playerShip; //reference to the Player Ship
    public GameObject EnemyGO; //reference to the enemyGO
    public GameObject EnemyBulletGO; // reference to the enemybulletGO
    public GameObject enemySpawner; //reference to the enemy spawner
    public GameObject GameOverGO; //reference to the game over image
    public GameObject scoreUITextGO;//reference to the score text UI game object
    public GameObject TimeEncounterUITextGO; //reference to the time encounter text UI game object
    public GameObject GameTitleGO; //reference to the GameTitleGO

    public enum GameManagerState
    {
        Opening,
        GamePlay,
        GameOver,
    }

    GameManagerState GMState;
    // Start is called before the first frame update
    void Start()
    {
        GMState = GameManagerState.Opening;
    }

    //Function to update the game manager state
    void UpdateGameManagerState()
    {
        switch (GMState)
        {
            case GameManagerState.Opening:
                //Hide game over
                GameOverGO.SetActive(false);

                //Display the game title
                GameTitleGO.SetActive(true);

                //Set play button visible(active)
                playButton.SetActive(true);
                break;
            case GameManagerState.GamePlay:

                //Reset the score
                scoreUITextGO.GetComponent<GameScore>().Score = 0;

                //Hide play button on gameplay state
                playButton.SetActive(false);

                //Hide the game title
                GameTitleGO.SetActive(false);

                //set the player visisble(active) and init the player lives
                playerShip.GetComponent<PlayerControl>().Init();

                //Start enemy spawner
                enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();

                //Start increase enemy speed
                EnemyGO.GetComponent<EnemyControl>().StartIncreaseMovingSpeed();

                //Start increase enemy's bullet speed
                EnemyBulletGO.GetComponent<EnemyBullet>().StartIncreaseMovingSpeed();

                //Start the time counter
                TimeEncounterUITextGO.GetComponent<TimeCounter>().StartTimeEncounter();


                break;
            case GameManagerState.GameOver:

                //Stop the time counter
                TimeEncounterUITextGO.GetComponent<TimeCounter>().StopTimeEncounter();

                //Stop enemy spawner
                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemyspawner();

                //Stop increase enemy speed
                EnemyGO.GetComponent<EnemyControl>().ResetMovingSpeed();

                //Stop increase enemy's bullet speed
                EnemyBulletGO.GetComponent<EnemyBullet>().ResetMovingSpeed();

                //Display game over
                GameOverGO.SetActive(true);
                //Change game manager state to Opening state after 8 seconds
                Invoke("ChangeToOpeningState", 8f);
                break;
        }
    }

    //Functio to set the game manager state
    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    //Our Play button will cal this function when the user clicks the button.
    public void StartGamePlay()
    {
        GMState = GameManagerState.GamePlay;
        UpdateGameManagerState();
    }

    //Function to change game manager state to opening state
    public void ChangeToOpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);
    }
}
