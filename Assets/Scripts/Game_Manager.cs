using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour {

    public GameObject[] Players = new GameObject[10];
    public GameObject spawnPoint;
    public int index;

    public float gameTime = 600;

    private GameObject currentPlayer;

	// Use this for initialization
	void Start () {

        spawnPoint = GameObject.FindWithTag("Respawn");

        index = Random.Range(0, Players.Length);
        currentPlayer = Players[index];
        Instantiate(currentPlayer);
        currentPlayer.transform.position = spawnPoint.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        gameTime -= Time.deltaTime;

        if (gameTime <= 0)
        {
            Application.LoadLevel("Game over");
        }
	}
}
