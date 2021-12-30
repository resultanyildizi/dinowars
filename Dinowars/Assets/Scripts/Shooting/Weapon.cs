using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Weapon : NetworkBehaviour
{

    [SyncVar]
    public Player Player;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public int magazineSize = 0;
    public int damage = 0;
    public int bulletsPerTap;
    public float timeBetweenShooting = 0;
    public float range = 0;
    public float reloadTime = 0;
    public float timeBetweenShots = 0;
    public bool allowButtonHold;


    private int bulletsShot;
    private int bulletsLeft;


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
        Controls.Player.ShootByPressing.performed += ctx => Shoot();
    }


    public override void OnStartClient()
    {
        transform.SetParent(Player.transform.Find("Hand"));
        transform.localScale = new Vector3(2, 2, 2);
        Debug.Log("Bunun çalýþmamasý lazýmdý");
    }
   
    [ClientCallback]
    public void Update()
    {
        
    }

    [Command]
    public void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);

        
       
        bullet.transform.localScale = new Vector3(this.transform.parent.localScale.x, 1, 1);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * this.transform.parent.localScale.x * 400f, ForceMode2D.Force);
        
        NetworkServer.Spawn(bullet, connectionToClient);
    }

    //private void FireABullet()
    //{
    //    GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
    //    Destroy(bullet, 5f);
    //}

    //private void ResetShot()
    //{
    //    readyToShoot = true;
    //}

    //private void Reload()
    //{
    //    Debug.Log("Reloading...");
    //    reloading = true;
    //    Invoke("ReloadFinished", reloadTime);
    //}
    //private void ReloadFinished()
    //{
    //    Debug.Log("Reloaded");
    //    bulletsLeft = magazineSize;
    //    reloading = false;
    //}

    //public bool GetInput()
    //{
    //    if (allowButtonHold) return Input.GetKey(KeyCode.Mouse0);
    //    else return Input.GetKeyDown(KeyCode.Mouse0);
    //}
}



//Spread
//float x = Random.Range(-spread, spread);
//float y = Random.Range(-spread, spread);

//Calculate Direction with Spread
//Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

//RayCast
//if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
//{
//    Debug.Log(rayHit.collider.name);

//    if (rayHit.collider.CompareTag("Enemy"))
//        rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
//}