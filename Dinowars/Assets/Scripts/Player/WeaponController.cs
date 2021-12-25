using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class WeaponController : NetworkBehaviour
{
    [SerializeField] Weapon weaponPrefab;
    float MAX_ANGLE = 30f;
    private Weapon weapon;



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

       
    }

    [ClientCallback]
    private void Update()
    {
        if (weapon == null) return;
        if (!hasAuthority) return;
       
        var lookDirection = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - weapon.transform.parent.transform.parent.position).normalized;
        
        var  lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        weapon.transform.eulerAngles = new Vector3(0, 0, lookAngle);

        Debug.Log(lookAngle);
        Vector3 localScale = Vector3.one;
        if (lookAngle>90 || lookAngle<-90)
        {
            localScale.y = -1f;
            
        }
        else
        {
            localScale.y = +1f;
        }

        weapon.transform.localScale = localScale;




    }
}
