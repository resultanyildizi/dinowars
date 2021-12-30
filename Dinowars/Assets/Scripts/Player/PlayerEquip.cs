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


    [Command]
    private void SpawnWeapon()
    {

        if (weaponType == WeaponType.GUN)
        {


            var gunPoint = this.transform.transform.Find("Hand").transform;
            var gun = Instantiate(gunPrefab, gunPoint.position, Quaternion.identity);
            gun.transform.localScale = new Vector3(2, 2, 2);

            var weapon = gun.GetComponent<Weapon>();
            weapon.Player = GetComponent<Player>();
            NetworkServer.Spawn(gun.gameObject, connectionToClient);

            
        }
        if (weaponType == WeaponType.RIFLE)
        {


            var gunPoint = this.transform.transform.Find("Hand").transform;
            var gun = Instantiate(riflePrefab, gunPoint.position, Quaternion.identity);
            gun.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            
            var weapon = gun.GetComponent<Weapon>();
            weapon.Player = GetComponent<Player>();
            NetworkServer.Spawn(gun.gameObject, connectionToClient);
            
            NetworkServer.Destroy(this.transform.GetChild(0).Find("Gun(Clone)").gameObject); // Rifle aldýðýnda elindeki pistolu yok eden kod

        }
       /* if (this.transform.GetChild(0).transform.childCount>0)
        {

        }       
        Debug.Log("Mevcut Silah: " + this.transform.GetChild(0).Find("Gun(Clone)"));*/

    }


    [Command]
    public void CmdChangeWeaponType(WeaponType type)
    {
        weaponType = type;
    }
}

