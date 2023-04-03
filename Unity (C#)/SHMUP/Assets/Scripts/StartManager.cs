using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject vehicle;
    public GameObject text;
    public GameObject highScore;
    public GameObject button;

    public bool addedEvent;
    public bool inGame;
    public float score;
    public float highscore;

    public string format;

    private GameObject[] game;

    private void Awake()
    {
        // Sees if a GameManger is already initalized, and if so delete this one
        game = GameObject.FindGameObjectsWithTag("GameController");

        if (game.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Gets a refrence to a player and text if the scene is SHMUP
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SHMUP") &&
            vehicle == null || text == null)
        {
            vehicle = GameObject.FindWithTag("Player");
            text = GameObject.FindWithTag("Text");
        }

        // Gets a refrece to a button and highscore if the scene is MainMenu
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu") &&
            highScore == null || button == null)
        {
            button = GameObject.FindWithTag("Button");
            highScore = GameObject.FindWithTag("Highscore");
        }

        // During game update the score variable
        if (inGame && vehicle != null)
        {
            text.GetComponent<Stats>().MaxScore = score;
            score = text.GetComponent<Stats>().Score;

            // if the health of the vehicle reaches 0 then go back to the main menu
            if (vehicle.GetComponent<Vehicle>().Health < 0)
            {
                score = text.GetComponent<Stats>().Score;
                SceneManager.LoadScene("MainMenu");
                inGame = false;
                addedEvent = false;
                if (score < highscore)
                {
                    score = highscore;
                }
            }
        }

        if (!inGame && highScore != null && button != null)
        {
            if (!addedEvent)
            {
                button.GetComponent<Button>().onClick.AddListener(OnButton);               
                addedEvent = true;
            }
            score = Mathf.Floor(score);
            format = string.Format($"Highscore: {score}");
            highScore.GetComponent<Text>().text = format;
        }
    }

    /// <summary>
    /// The logic to start the game
    /// </summary>
    public void OnButton()
    {
        highscore = score;
        SceneManager.LoadScene("SHMUP");
        inGame = true;
    }
}
