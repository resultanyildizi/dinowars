using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private float speed;
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
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 12);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = false;
    }

    
}
