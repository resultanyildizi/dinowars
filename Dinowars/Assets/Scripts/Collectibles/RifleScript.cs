using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RifleScript : NetworkBehaviour
{
    private Rigidbody2D body;
    private PlayerEquip onEquip;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Body") || collision.CompareTag("Foot") )
        {
            Player player = collision.GetComponentInParent<Player>();
            onEquip = player.GetComponent<PlayerEquip>();


            onEquip.riflePickedUp();

            GameObject.Destroy(gameObject);
            NetworkServer.Destroy(gameObject);

            /*onEquip = player.GetComponent<PlayerEquip>();
            onEquip.OnWeaponChanged(WeaponType.NONE,WeaponType.RIFLE);
            NetworkServer.Destroy(gameObject);
            GameObject.Destroy(gameObject);*/
        }
    }
}
