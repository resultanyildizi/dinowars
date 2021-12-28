using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private double damage = 30;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
        }

        Destroy();
    }

    [Server]
    private void Destroy()
    {
        NetworkServer.Destroy(this.gameObject);
        GameObject.Destroy(this.gameObject);
    }


}
