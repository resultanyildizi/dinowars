using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    [SerializeField] Weapon weaponPrefab;


    private Weapon weapon;
    private Rigidbody2D rb;

    private float gunOffsetX;
    private float gunOffsetY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon =  Instantiate<Weapon>(weaponPrefab, rb.transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if(weapon.GetInput())
        {
            weapon.Shoot();
        }
    }

    void Start()
    {
        weapon.transform.SetParent(parent: this.transform, worldPositionStays: true);
    }

    private void OnDestroy()
    {
        Object.Destroy(weapon);
    }
}
