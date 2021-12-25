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
        CmdChangeWeaponType(WeaponType.GUN);
    }
    private void OnWeaponChanged(WeaponType oldWeapon, WeaponType newWeapon)
    {
        if (!hasAuthority) return;
        SpawnWeapon();
    }

    [Command]
    private void SpawnWeapon()
    {
        var gunPoint = this.transform.transform.Find("Hand").transform;

        var gun = Instantiate(gunPrefab, gunPoint.position, Quaternion.identity);
        gun.transform.localScale = new Vector3(2, 2, 2);
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


