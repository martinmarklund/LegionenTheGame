using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    // Public variables
    public float speed = 5;
    public float gravity = 10;
    public float verticalSpeed = 0;
    public float jumpSpeed = 8;

    // Private variables
    private CharacterController controller;
    private Vector2 velocity;

	// Use this for initialization
	void Start () {

        controller = GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update () {

        velocity = transform.forward * Input.GetAxis("Vertical") * speed;
        
        if(controller.isGrounded)
        {
            verticalSpeed = 0;
            if (Input.GetKeyDown("space"))
            {
                verticalSpeed = jumpSpeed;
            }
        }

        verticalSpeed -= gravity * Time.deltaTime;
        velocity.y = verticalSpeed;
        controller.Move(velocity * Time.deltaTime);

	}
}
