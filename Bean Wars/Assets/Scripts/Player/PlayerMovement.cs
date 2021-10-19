using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float speed;
    private bool grounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //horizantalInput is always 0, if click the right button, it should be 1, and -1
        float horizantalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizantalInput * speed, body.velocity.y);

        /* if (horizantalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizantalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1); */


        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, 5);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}
