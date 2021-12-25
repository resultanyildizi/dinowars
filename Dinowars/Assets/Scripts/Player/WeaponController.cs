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
    float distance,mdistance;



    public override void OnStartClient()
    {   Vector3 spawnTransform = this.transform.Find("PlayerBody").Find("Hands").transform.position;
        weapon = Instantiate(weaponPrefab,spawnTransform , Quaternion.identity);
        weapon.transform.parent = this.transform;

        var ntChildren = transform.GetComponents<NetworkTransformChild>();

        foreach(var nt in ntChildren)
        {
            if (nt.target == null)
            {
                nt.target = weapon.transform;
            }
        }
        
        distance = weapon.transform.parent.Find("PlayerBody").Find("Hands").transform.position.x - weapon.transform.parent.Find("PlayerBody").transform.position.x;
        mdistance = distance * -1;
    }

    [ClientCallback]
    private void Update()
    {
        if (weapon == null) return;
        if (!hasAuthority) return;


       
        var lookDirection = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - weapon.transform.parent.position).normalized;
        
        var  lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        weapon.transform.eulerAngles = new Vector3(0, 0, lookAngle);

        
        Debug.Log(weapon.transform.parent.Find("PlayerBody").Find("Hands").transform.localPosition);
        Vector3 localScale = Vector3.one;
        Vector3 localPos = new Vector3(weapon.transform.localPosition.x , weapon.transform.localPosition.y, weapon.transform.localPosition.z);
        if (lookAngle>90 || lookAngle<-90)
        {
            weapon.transform.localScale = new Vector3(1, -1, 1);
            weapon.transform.localPosition = new Vector3( mdistance, weapon.transform.localPosition.y, weapon.transform.localPosition.z);
        }
        else
        {
            weapon.transform.localScale = new Vector3(1, 1, 1);
            weapon.transform.localPosition = new Vector3(distance, weapon.transform.localPosition.y, weapon.transform.localPosition.z);
        }
        
      





    }
}
