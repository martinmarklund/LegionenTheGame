using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

    // Public variables
    public float topSpeed = 2.0f;       // How fast the player can move
    public float jumpForce = 3.0f;      // Force applied to player when jumping
    public LayerMask playerMask;
    public bool canMoveInAir = true;

    // Private variables
    private float move;                 // Float holding move direction
    private bool facingRight = true;    // Is the player facing right
    private Animator animator;          // Reference to animator
    private bool isGrounded = false;    // Not grounded by default

    Rigidbody2D myBody;
    Transform myTrans, tagGround;

    void Start()
    {
        myBody = this.GetComponent<Rigidbody2D>();
        myTrans = this.transform;
        tagGround = GameObject.Find(this.name + "/tag_ground").transform;

        animator = GetComponent<Animator>();
    }

    // Physics will be manipulated at the end of each fram in fixed update
    void FixedUpdate()
    {
        // Will return true or false depending on wether the player collids with something or not
        isGrounded = Physics2D.Linecast(myTrans.position, tagGround.position, playerMask);

        // Get move directions
        Move(Input.GetAxis("Horizontal"));

        // Check if player is jumping
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
           
    }

    void Update()
    {
        
    }

    // Move the player
    public void Move(float horizontalInput)
    {
        // Check if the player is in the air
        if (!canMoveInAir && !isGrounded)
            return;

        // Adds velocity to the Rigidbody in the move direction multiplied with speed.
        myBody.velocity = new Vector2(horizontalInput * topSpeed, myBody.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        // Check if the sprite needs to be flipped
        if (horizontalInput > 0 && !facingRight)
            Flip();
        else if (horizontalInput < 0 && facingRight)
            Flip();
    }

    public void Jump()
    {
        if (isGrounded)
        {
            myBody.velocity += jumpForce * Vector2.up;
            Debug.Log("Jumped");
        }
        animator.SetTrigger("Jump");
    }


    /// <summary>
    /// Flips the sprite over the x axis
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;

        // Get local scale and flip over the x axis
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;

        // Update local scale
        transform.localScale = theScale;
    }
}