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
    public float health = 3;
    public float invincibleAfterHurt = 2;
    public float damageCooldown = 5;

    // Sound effects
    public AudioClip jumpSound1;
    public AudioClip jumpSound2;
    public AudioClip pineappleSound;
    public AudioClip cubesSound;
    public AudioClip brooshSound;
    public AudioClip hurtSound;
    public AudioClip gameOverSound;
    public AudioClip nollanSound;

    [HideInInspector]
    public Collider2D[] myColls;

    public int score = 0;

    // Private variables
    float move;                 // Float holding move direction
    bool facingRight = true;    // Is the player facing right
    Animator animator;          // Reference to animator
    bool isGrounded = false;    // Not grounded by default
    bool isShielded = false;    // Not shielde by default
    float timeShield = 10.0f;   // Duration of Broosh power
    float timeStronger = 10.0f; // Duration of Cube power
    int enemyLayer;
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

        // Reset and reenable collision after power up
        if (timeShield <= 0)
        {
            isShielded = !isShielded;
            timeShield = 0.0f;
            Physics2D.IgnoreLayerCollision(enemyLayer, gameObject.layer, false);
        }
        if (timeStronger <= 0)
        {
            topSpeed = 2.5f;
            jumpForce = 7.0f;
            timeStronger = 0.0f;
        }
    }

    void Update()
    {
        score = PlayerStats.Score;
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
            SoundManager.instance.RandomizeSfx(jumpSound1, jumpSound2);
        }
    }


    // Used for sound triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Pineapple")
        {
            SoundManager.instance.PlaySingle(pineappleSound);
        }
        else if (other.tag == "Cubes")
        {
            SoundManager.instance.PlaySingle(cubesSound);
        }
        else if (other.tag == "Broosh")
        {
            SoundManager.instance.PlaySingle(brooshSound);
        }
        else if(other.tag == "Nollan")
        {
            Debug.Log("Nollan");
            SoundManager.instance.PlaySingle(nollanSound);
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
        enemyLayer = LayerMask.NameToLayer("Enemy");
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
        SoundManager.instance.PlaySingle(hurtSound);
        Debug.Log("Healh = " + health);
        if(health <= 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Application.LoadLevel("Game over");
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
        if(enemy != null)
        {
            bool enemyHurt = false;
            enemyLayer = collision.gameObject.layer;
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
            if(!enemyHurt && !isShielded)
            {
                Hurt();
            }
            if(isShielded)
            {
                // Ignore layer "Enemy"
                Physics2D.IgnoreLayerCollision(enemyLayer, gameObject.layer, true);
            }
        }

        if (collision.collider.tag == "Kill zone")
            Application.LoadLevel("Game over");
        // TODO: Fix so that the player doesn't die when getting hit by falling objects. 
    }

    public IEnumerator StartCountdownStronger()
    {
        while(timeStronger >= 0)
        {
            if(timeStronger <= 0)
            {
                yield break;
            }
            yield return new WaitForSeconds(1.0f);
            timeStronger--;
        }
        yield return null;
    }

    public IEnumerator StartCountdownShield()
    {
        while (timeShield >= 0)
        {
            if(timeShield <= 0)
            {
                yield break;
            }
            yield return new WaitForSeconds(1.0f);
            timeShield--;
        }
        yield return null;
    }

    public void AddHealth(float healthAmount)
    {
        health += healthAmount;
        Debug.Log("POWER UP: Health " + health);
    }

    public void FasterStronger()
    {
        // Use IEnumerator to have a periodic power up
        topSpeed = 2.0f;
        jumpForce = 8.0f;

        timeStronger = 10.0f;
        Debug.Log("POWER UP: Faster Stronger");
        StartCoroutine(StartCountdownStronger());
    }

    public void Shielded()
    {
        isShielded = true;
        timeShield = 10.0f;
        Debug.Log("POWER UP: Shield " + isShielded);
        StartCoroutine(StartCountdownShield());
    }
}
