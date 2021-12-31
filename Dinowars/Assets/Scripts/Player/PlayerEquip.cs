using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public enum WeaponType : byte
{
    NONE,
    GUN,
    RIFLE
}

public class PlayerEquip : NetworkBehaviour
{
    [SerializeField] private Weapon gunPrefab;
    [SerializeField] private Weapon riflePrefab;

    [SyncVar(hook = nameof(OnWeaponChanged))]
    public WeaponType weaponType = WeaponType.NONE;

    private GameObject currentWeapon = null;


    public override void OnStartAuthority()
    {
        CmdChangeWeaponType(WeaponType.GUN);
    }
    private void OnWeaponChanged(WeaponType oldWeapon, WeaponType newWeapon)
    {
        if (!hasAuthority) return;
        SpawnWeapon();
    }
    public void riflePickedUp()
    {
        if (!hasAuthority) return;
        CmdChangeWeaponType(WeaponType.RIFLE);
    }

    [ClientRpc]
    private void CRpcPlayEquip()
    {
        FindObjectOfType<AudioController>().Play("EquipWeapon");
    }


    [Command]
    private void SpawnWeapon()
    {
        var gunPoint = this.transform.transform.Find("Hand").transform;

        var dir = transform.Find("PlayerBody").localScale.x > 0 ? 1 : -1;

        if (weaponType == WeaponType.GUN)
        {
            currentWeapon = Instantiate(gunPrefab.gameObject, gunPoint.position, Quaternion.identity);
            currentWeapon.GetComponent<Weapon>().Player = GetComponent<Player>();
            currentWeapon.transform.localScale = new Vector3( dir * 2, 2, 2);
            NetworkServer.Spawn(currentWeapon.gameObject, connectionToClient);
            return;
        }
        if (weaponType == WeaponType.RIFLE)
        {
            NetworkServer.Destroy(currentWeapon.gameObject);
            currentWeapon = Instantiate(riflePrefab.gameObject, gunPoint.position, Quaternion.identity);
            currentWeapon.GetComponent<Weapon>().Player = GetComponent<Player>();
            currentWeapon.transform.localScale = new Vector3(dir * .2f, .2f, .2f);
            NetworkServer.Spawn(currentWeapon.gameObject, connectionToClient);
            return;
        }

    }


    [Command]
    public void CmdChangeWeaponType(WeaponType type)
    {
        weaponType = type;
        CRpcPlayEquip();
    }
}

