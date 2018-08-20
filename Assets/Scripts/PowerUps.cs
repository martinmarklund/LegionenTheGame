using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour {

    public float healthAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("tag = " + other.tag);
        if(other.tag == "Player")
        {
            Player_Controller player = other.gameObject.GetComponent<Player_Controller>();

            // Add health to player
            if(name.Contains("apple"))
            {
                Debug.Log("PINAPPLE!!!");
                player.AddHealth(healthAmount);
                PlayerStats.Score = 1000;
                Destroy(gameObject);
            }
            // Shield player once
            if(name.Contains("Broosh"))
            {
                Debug.Log("BROOOOOOOSH!!!");
                PlayerStats.Score = 500;
                player.Shielded();
                Destroy(gameObject);
            }
            // Faster, stronger
            if(name.Contains("Cubes"))
            {
                Debug.Log("CUUUUUBE!!!");
                PlayerStats.Score = 500;
                player.FasterStronger();
                Destroy(gameObject);
            }
            if(name.Contains("Nollan"))
            {
                Debug.Log("NÄMEN HEEEEJ NOLLAN");
                player.score += 5000;
            }
        }
    }
}
