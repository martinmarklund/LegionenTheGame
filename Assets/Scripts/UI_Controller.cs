using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour {

    public Text healthText;
    public Text scoreText;

    Player_Controller playerScript;
    GameObject player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player_Peggy");
        //player = GameObject.FindWithTag("Player");
        if (player.tag == "Player")
            Debug.Log("Player found");
        playerScript = player.GetComponent<Player_Controller>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerScript.health <= 0)
            healthText.text = "Oj oj";
        else
            healthText.text = "x " + playerScript.health.ToString();

        scoreText.text = "SCORE: " + playerScript.score.ToString();

    }
}
