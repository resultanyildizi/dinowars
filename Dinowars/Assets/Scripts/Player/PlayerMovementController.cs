using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 200f;
    [SerializeField] private float jumpFactor = 5f;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] Weapon defaultWeapon;
    Weapon weapon;
    private bool grounded;
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
        Controls.Player.Shoot.performed += ctx => Shoot();
    }

    public override void OnStartClient()
    {

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
    }

    [Client]
    private void Jump()
    {
        //if (grounded)
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpFactor);
    }

    [Client]
    private void Shoot()
    {
        //weapon.Shoot();
    }


}
