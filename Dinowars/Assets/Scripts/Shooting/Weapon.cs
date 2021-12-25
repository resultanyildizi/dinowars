using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Weapon : MonoBehaviour
{
    //Gun stats
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

  
    //bools 
    bool readyToShoot, reloading;

    //Reference
    //public Camera fpsCam;

    private void Awake()
    {
        reloading = false;
        readyToShoot = true;
        bulletsLeft = magazineSize;
    }
  
    public void Shoot()
    {
        if (bulletsLeft <= 0 && !reloading) Reload();

        //Shoot
        if (readyToShoot && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            readyToShoot = false;

            FireABullet();

            bulletsLeft--;
            bulletsShot--;

            Invoke("ResetShot", timeBetweenShooting);

            for (int i = 1; i <= bulletsShot; i++)
            {
                if (bulletsLeft > 0)
                {
                    bulletsLeft--;
                    Invoke("FireABullet", timeBetweenShots * i);
                }
            }

        }
    }

    private void FireABullet()
    {
        /*GameObject bullet = Instantiate(bulletPrefab, new Vector2(firePoint.transform.position.x,firePoint.transform.position.y), firePoint.transform.rotation);
        Destroy(bullet, 5f);
        */
        Debug.Log("Ateþ");
    }


    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        Debug.Log("Reloading...");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        Debug.Log("Reloaded");
        bulletsLeft = magazineSize;
        reloading = false;
    }

    public bool GetInput()
    {
        if (allowButtonHold) return Input.GetKey(KeyCode.Mouse0);
        else return Input.GetKeyDown(KeyCode.Mouse0);
    }
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