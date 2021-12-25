using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public enum WeaponType: byte
{
    NONE,
    GUN
}

public class PlayerEquip : NetworkBehaviour
{
    [SerializeField] private Weapon gunPrefab;


    [SyncVar(hook = nameof(OnWeaponChanged))]
    WeaponType weaponType = WeaponType.NONE;


    public override void OnStartAuthority()
    {
        Debug.Log("I HAVE AUTGORITY");
        StartCoroutine(ChangeWeaponCoroutine());
    }

    IEnumerator ChangeWeaponCoroutine()
    {
        yield return new WaitForSeconds(2);
        CmdChangeWeaponType(WeaponType.GUN);
    }

    private void OnWeaponChanged(WeaponType oldWeapon, WeaponType newWeapon)
    {
        Debug.Log("Creating Weapon: " + hasAuthority);

        if (!hasAuthority) return;
        SpawnWeapon();
    }

    [Command]
    private void SpawnWeapon()
    {
        var gunPoint = this.transform.Find("PlayerBody").transform.Find("Hand").transform;

        var gun = Instantiate(gunPrefab, gunPoint.position, Quaternion.identity);

        var weapon = gun.GetComponent<Weapon>();

        weapon.Player = GetComponent<Player>();
        
        NetworkServer.Spawn(gun.gameObject, connectionToClient);
    }


    [Command]
    public void CmdChangeWeaponType(WeaponType type)
    {
        weaponType = type;
    }
}


