using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
public class GunDirection : NetworkBehaviour
{
    private static float MAX_ANGLE = 60f;
    Transform firepoint;
    float lookAngle;
    Vector2 lookDirection;
    private void Awake()
    {
        firepoint = transform;
    }

    override
    public void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    void Update()
    {
       /* lookDirection = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(transform.parent.position.x, transform.parent.position.y);
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        if (lookDirection.x - transform.position.x >= 0)
        {
            // if (lookAngle >= MAX_ANGLE) lookAngle = MAX_ANGLE;
            this.transform.parent.rotation = Quaternion.Euler(0, 0, lookAngle);
        }
        else
        {  // if (lookAngle - Mathf.Sign(lookAngle) * 180 <= -MAX_ANGLE && hasAuthority) lookAngle = 180 - MAX_ANGLE;
            firepoint.rotation = Quaternion.Euler(180f, 0, -lookAngle);
        }*/

    }
}
