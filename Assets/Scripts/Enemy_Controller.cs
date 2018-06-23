using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{

    // Public variables
    public float speed;
    public LayerMask enemyMask;
    Rigidbody2D m_body;
    Transform m_trans;
    float m_width, m_height;

    // Use this for initialization
    void Start()
    {
        m_trans = this.transform;
        m_body = this.GetComponent<Rigidbody2D>();
        // Get width and height of sprite
        SpriteRenderer mySprite = this.GetComponent<SpriteRenderer>();
        m_width = mySprite.bounds.extents.x;
        m_height = mySprite.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Check to see if there's ground in front of us before moving forward
        Vector2 lineCastPos = m_trans.position.toVector2() - m_trans.right.toVector2() * m_width + Vector2.up * m_height;
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
        Debug.DrawLine(lineCastPos, lineCastPos - m_trans.right.toVector2() * 0.05f);
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - m_trans.right.toVector2() * 0.05f, enemyMask);

        // If there's no ground, turn around. Or if it's blocked, turn around.  
        if (!isGrounded || isBlocked)
        {
            Vector3 currRot = m_trans.eulerAngles;
            currRot.y += 180;
            m_trans.eulerAngles = currRot;
        }

        // Always move forward
        Vector2 m_vel = m_body.velocity;
        m_vel.x = -m_trans.right.x * speed;
        m_body.velocity = m_vel;
    }

    void moveEnemy()
    {
        float translate = Input.GetAxis("Horizontal");
    }
}
