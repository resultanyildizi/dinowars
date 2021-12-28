using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class GunDirection : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        //enabled = true;
    }

    private static float MAX_ANGLE = 30f;

    [ClientCallback]
    void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.parent.position);

        mousePos.x = mousePos.x - gunPos.x;
        mousePos.y = mousePos.y - gunPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;


        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            if (angle >= MAX_ANGLE) angle = MAX_ANGLE;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
        else
        {
            if (angle - Mathf.Sign(angle) * 180 <= -MAX_ANGLE) angle = 180 - MAX_ANGLE;
            transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, -angle));
        }
    }
}
