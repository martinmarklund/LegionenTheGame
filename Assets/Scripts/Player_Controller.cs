using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    // Public variables
    public double gravity = 9.8;
    public float verticalSpeed = 0;

    // Private variables
    private CharacterController controller;

	// Use this for initialization
	void Start () {

        controller = GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update () {
		
        if(controller.isGrounded)
        {
            verticalSpeed = 0;
        }


	}
}
