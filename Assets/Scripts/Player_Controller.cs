using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    // Public variables
    public float topSpeed = 2.0f;       // How fast the player can move
    public float jumpForce = 700.0f;    // Force applied to player when jumping
    public Transform groundCheck;       // Transform at player's feet to check if player is grounded
    public LayerMask whatIsGround;     // What layer is considered the ground

    // Private variables
    private float move;                 // Float holding move direction
    private bool facingRight = true;    // Is the player facing right
    private Animator animator;          // Reference to animator
    private bool grounded = false;      // Not grounded by default
    public float groundRadius = 0.2f;  // The radius of the circle used for checking the distance to the ground


    void Start()
    {
        animator = GetComponent<Animator>();
        groundCheck = GetComponent<Transform>();
        whatIsGround = 8;
        Debug.Log(whatIsGround.value);
    }
    // Physics will be manipulated at the end of each fram in fixed update
    void FixedUpdate()
    {
        Debug.Log(groundCheck.position);
        // Will return true or false depending on whether groundCheck hit whatIsGround with the groundRadius
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        //animator.SetBool("Ground", grounded);
        //animator.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y); 
        
        // Get move directions
        move = Input.GetAxis("Horizontal");

        // Adds velocity to the Rigidbody in the move direction multiplied with speed.
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * topSpeed, GetComponent<Rigidbody2D>().velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(move));

        // Check if the sprite needs to be flipped
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        // Can the player jump, if yes: add jump force
        if (grounded && Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("Grounded", false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
        }

    }

    void Update()
    {

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
