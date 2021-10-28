using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpFactor;
    private Rigidbody2D rigidBody;
    private bool grounded;


    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizantalInput = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(horizantalInput * speed, rigidBody.velocity.y);

        //horizantalInput is always 0, if click the right button, it should be 1, and -1

        /* if (horizantalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizantalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1); */


        if (Input.GetKeyDown(KeyCode.Space))
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.otherCollider.CompareTag("Foot"))
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            return;
        }
        if (collision.gameObject.CompareTag("Ground"))
            grounded = false;
    }

    
}
