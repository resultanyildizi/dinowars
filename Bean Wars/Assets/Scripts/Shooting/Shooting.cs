using System;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform firePoint;

    private void Update()
    {
        //Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //float rotateY = 0f;

        //if (mousePos.x < transform.position.x)
        //{
        //    rotateY = 180f;
        //}

        //transform.eulerAngles = new Vector3(transform.rotation.x,rotateY,transform.rotation.z);
       
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        Destroy(bullet, 5f);
    }
}
