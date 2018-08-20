using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour {

    public Text healthText;
    public Text scoreText;
    public Text timeText;

    private Player_Controller playerScript;
    private GameObject player;
    private Game_Manager managerScript;
    private GameObject manager;

    private bool folkparken;

    // Use this for initialization
    void Start()
    {
        if (Application.loadedLevelName == "Folkparken")
            folkparken = true;
        else
            folkparken = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(folkparken)
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
                playerScript = player.GetComponent<Player_Controller>();
                Debug.Log("Player found!");
            }
            if (manager == null)
            {
                manager = GameObject.Find("Game Manager");
                managerScript = manager.GetComponent<Game_Manager>();
                Debug.Log("Manager found!");
            }


            if (playerScript.health <= 0)
                healthText.text = "Oj oj";
            else
                healthText.text = "x " + playerScript.health.ToString();

            scoreText.text = "SCORE: " + PlayerStats.Score.ToString();

            timeText.text = "TIME: " + managerScript.gameTime.ToString("0");
        }
    }
}
