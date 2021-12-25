using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class WeaponController : NetworkBehaviour
{
    [SerializeField] Weapon weaponPrefab;

    private Weapon weapon;

    private float gunOffsetX;
    private float gunOffsetY;

    public override void OnStartClient()
    {
        weapon = Instantiate(weaponPrefab, this.transform.Find("PlayerBody").Find("Hands").transform.position, Quaternion.identity);
        weapon.transform.parent = this.transform.Find("PlayerBody").Find("Hands").transform;

        var ntChildren = transform.GetComponents<NetworkTransformChild>();

        foreach(var nt in ntChildren)
        {
            if (nt.target == null)
            {
                nt.target = weapon.transform;
            }
        }

        Debug.Log("WEAPON CREATED");
    }

    [ClientCallback]
    private void Update()
    {
        if (weapon == null) return;
        if (!hasAuthority) return;
        var lookDirection = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(weapon.transform.parent.position.x, weapon.transform.parent.position.y);
       var  lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        if (lookDirection.x - weapon.transform.position.x >= 0)
        {
            // if (lookAngle >= MAX_ANGLE) lookAngle = MAX_ANGLE;
            this.weapon.transform.rotation = Quaternion.Euler(0, 0, lookAngle);
        }
        else
        {  // if (lookAngle - Mathf.Sign(lookAngle) * 180 <= -MAX_ANGLE && hasAuthority) lookAngle = 180 - MAX_ANGLE;
            this.weapon.transform.rotation = Quaternion.Euler(180f, 0, -lookAngle);
        }
    }
}
