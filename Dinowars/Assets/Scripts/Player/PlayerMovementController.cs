using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 200f;
    [SerializeField] private float jumpFactor = 5f;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private Animator animator;


    private float inputValue;

    private Controls controls;

    private Controls Controls
    {
        get
        {
            if(controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;

        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<float>());
        Controls.Player.Move.canceled += ctx => ResetMovement();
        Controls.Player.Jump.performed += ctx => Jump();
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    [ClientCallback]
    private void OnDisable() => Controls.Disable();
    [ClientCallback]
    private void FixedUpdate() => Move();

    [Client]
    private void SetMovement(float value) => inputValue = value;

    [Client]
    private void ResetMovement() => inputValue = 0;

    [Client]
    private void Move()
    {
        rigidbody2D.velocity = new Vector2( inputValue * movementSpeed * Time.fixedDeltaTime, rigidbody2D.velocity.y);
        float horizontalMove = Input.GetAxisRaw("Horizontal") * movementSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    [Client]
    private void Jump()
    {
         rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpFactor);
       
    }

   private bool IsGrounded()
    {
        
        return transform.Find("Foot").GetComponent<GroundCheck>().isGrounded;
    }

}
