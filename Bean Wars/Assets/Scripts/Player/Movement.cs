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
        float horizantalInput = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(horizantalInput * speed, rigidBody.velocity.y);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (mousePos.x >= transform.position.x)       
             transform.localScale = Vector3.one;
        else
            transform.localScale = new Vector3(-1, 1, 1);

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
