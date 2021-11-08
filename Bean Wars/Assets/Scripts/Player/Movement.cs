using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpFactor;
    private Rigidbody2D rigidBody;
    private bool grounded;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move character
        float horizantalInput = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(horizantalInput * speed, rigidBody.velocity.y);

        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();

    }

    private void Jump()
    {
        if(grounded)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpFactor);
            grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.otherCollider.CompareTag("Foot"))
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            return;
        }

        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }    
}
