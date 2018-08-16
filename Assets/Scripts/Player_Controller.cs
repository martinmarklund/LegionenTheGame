using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // Public variables
    //public static Player_Controller instance;
    public LayerMask playerMask;
    public bool canMoveInAir = true;

    // Movement
    public float topSpeed = 2.0f;       // How fast the player can move
    public float jumpForce = 3.0f;      // Force applied to player when jumping

    // Combat
    public int health = 3;
    public float invincibleAfterHurt = 2;
    public float damageCooldown = 5;

    [HideInInspector]
    public Collider2D[] myColls;

    // Private variables
    float move;                 // Float holding move direction
    bool facingRight = true;    // Is the player facing right
    Animator animator;          // Reference to animator
    bool isGrounded = false;    // Not grounded by default
    Rigidbody2D myBody;
    Transform myTrans, tagGround;

    // Jump
    bool jumped;
    float jumpTime = 0f;        // Used as a timer for when trigger "Land" will activate
    public float jumpDelay = 0.5f;

    // Initiation
    void Start()
    {
        //instance = this;
        myColls = this.GetComponents<Collider2D>();
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
            jumpTime = jumpDelay;
            Jump();
            jumped = true;
        }
        jumpTime -= Time.deltaTime; 

        if(jumpTime <= 0 && isGrounded && jumped)
        {
            animator.SetTrigger("Land");
            jumped = false;
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

    // Jump the character by moving it upwards
    public void Jump()
    {
        if (isGrounded)
        {
            myBody.velocity += jumpForce * Vector2.up;
            animator.SetTrigger("Jump");
        }
    }

    // Flips the sprite over the x axis
    void Flip()
    {
        facingRight = !facingRight;

        // Get local scale and flip over the x axis
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;

        // Update local scale
        transform.localScale = theScale;
    }

    // Action when player gets hurt
    public void TriggerHurt(float hurtTime)
    {
        StartCoroutine(HurtBlinker(hurtTime));
    }

    // Counter for hurt animation
    IEnumerator HurtBlinker(float hurtTime)
    {
        // Ignore collision with enemies
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int playerLayer = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer);
        foreach(Collider2D col in myColls)
        {
            col.enabled = false;
            col.enabled = true;
        }

        // Start looping blink animation
        animator.SetLayerWeight(1, 1);

        // Wait for invincibility to end
        yield return new WaitForSeconds(hurtTime);

        // Stop blinking animation and re-enable collision
        Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer, false);
        animator.SetLayerWeight(1, 0);
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(damageCooldown);
    }

    // Remove health when hurt
    void Hurt()
    {
        health--;
        Debug.Log("Healh = " + health);
        if(health <= 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Application.LoadLevel(Application.loadedLevel);
        }
        else 
        {
            //StartCoroutine(Damage());
            TriggerHurt(invincibleAfterHurt);
        }
    }

    // Checks if there is a collision between the player and any enemies
    void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy_Controller enemy = collision.collider.GetComponent<Enemy_Controller>();
        Debug.Log(enemy.name);
        if(enemy != null)
        {
            bool enemyHurt = false;
            foreach(ContactPoint2D point in collision.contacts)
            {
                if(point.normal.y >= 0.9f)
                {
                    Vector2 velocity = myBody.velocity;
                    velocity.y = jumpForce/2;
                    myBody.velocity = velocity;
                    enemy.Hurt();
                    enemyHurt = true;
                }
            }
            if(!enemyHurt)
            {
                Hurt();
            }
        }

        // TO DO: Fix so that the player doesn't die when getting hit by falling objects. 
    }
}