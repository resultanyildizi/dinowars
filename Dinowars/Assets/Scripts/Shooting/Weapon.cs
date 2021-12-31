using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Weapon : NetworkBehaviour
{

    [SyncVar]
    public Player Player;


    [SerializeField] private WeaponType weaponType;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int damage = 0;
    [SerializeField] private float timeBetweenShooting = 0;
    [SerializeField] private float timeBetweenShots = 0;

    private float lastFired = 0;


    private Controls controls;

    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }


    public void OnEnable() => Controls.Enable();

    public void OnDisable() => Controls.Disable();


    public override void OnStartAuthority()
    {

        enabled = true;

        switch(weaponType)
        {
            case WeaponType.GUN:
                Controls.Player.ShootByPressing.performed += ctx => ShootGun();
                break;
            case WeaponType.RIFLE:
                Controls.Player.ShootByHolding.started += ctx => StartCoroutine(nameof(ShootRifle));
                Controls.Player.ShootByHolding.canceled += ctx => StopCoroutine(nameof(ShootRifle));
                break;
        }

    }


    public override void OnStartClient()
    {
        transform.SetParent(Player.transform.Find("Hand"));
        transform.localPosition = Vector3.zero;
    }

    

    private IEnumerator ShootRifle()
    {
        while(true)
        {
            Shoot();
            yield return new WaitForSeconds(0.4f);
        }
    }

    private void ShootGun() {
        var current = Time.time;
        if(current > lastFired + 0.6) {
            lastFired = current;
            Shoot();
        }
    }

    [ClientCallback]
    public void Update() { }

    [Command]
    public void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
        bullet.transform.localScale = new Vector3(this.transform.parent.localScale.x, 1, 1);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * this.transform.parent.localScale.x * 400f, ForceMode2D.Force);
        NetworkServer.Spawn(bullet, connectionToClient);
        CRpcPlayShootSound();
    }
    [ClientRpc]
    private void CRpcPlayShootSound()
    {
        if (weaponType == WeaponType.GUN)
        {
            FindObjectOfType<AudioController>().Play("Pistol");
        }
        else if (weaponType == WeaponType.RIFLE)
        {
            FindObjectOfType<AudioController>().Play("Rifle");
        }
    }


    //IEnumerator WaitShootTime() {
    //    yield return new WaitForSeconds(0.1f);
    //}
}