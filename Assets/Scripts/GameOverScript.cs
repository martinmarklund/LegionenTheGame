using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

    public Text scoreText;

    public int finalScore;

    private void Start()
    {
        finalScore = PlayerStats.Score;
        Debug.Log(finalScore);
        scoreText.text = "SCORE: " + finalScore.ToString();

    }

    void Update () {
        if (Input.GetKeyDown("space"))
        {
            PlayerStats.Score = -finalScore;
            Application.LoadLevel("Startskärm");
        }
            
	}
}
