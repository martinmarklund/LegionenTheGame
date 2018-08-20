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

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //player = GameObject.FindWithTag("Player");
        if (player.tag == "Player")
            Debug.Log("Player found");
        playerScript = player.GetComponent<Player_Controller>();

        manager = GameObject.Find("Game Manager");
        managerScript = manager.GetComponent<Game_Manager>();

    }

    // Update is called once per frame
    void Update()
    {

        if (playerScript.health <= 0)
            healthText.text = "Oj oj";
        else
            healthText.text = "x " + playerScript.health.ToString();

        scoreText.text = "SCORE: " + playerScript.score.ToString();

        timeText.text = managerScript.gameTime.ToString("0");


    }
}
