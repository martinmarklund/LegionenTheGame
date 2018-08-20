/*<<<<<<< HEAD
﻿using System.Collections.Generic;
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
                Destroy(gameObject);
            }
            // Shield player once
            if(name.Contains("Broosh"))
            {
                Debug.Log("BROOOOOOOSH!!!");
                player.Shielded();
                Destroy(gameObject);
            }
            // Faster, stronger
            if(name.Contains("Cubes"))
            {
                Debug.Log("CUUUUUBE!!!");
                player.FasterStronger();
                Destroy(gameObject);
            }
        }
    }
}
=======
*/
﻿using System.Collections.Generic;
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
                // player.score += 1000;
                PlayerStats.Score = 1000;
                Destroy(gameObject);
            }
            // Shield player once
            if(name.Contains("Broosh"))
            {
                Debug.Log("BROOOOOOOSH!!!");
                // player.score += 500;
                PlayerStats.Score = 500;
                player.Shielded();
                Destroy(gameObject);
            }
            // Faster, stronger
            if(name.Contains("Cubes"))
            {
                Debug.Log("CUUUUUBE!!!");
                //player.score += 500;
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
