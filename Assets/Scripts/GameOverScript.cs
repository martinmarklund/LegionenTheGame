using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour {

	void Update () {
        if (Input.GetKeyDown("space"))
            Application.LoadLevel("Startskärm");
	}
}
