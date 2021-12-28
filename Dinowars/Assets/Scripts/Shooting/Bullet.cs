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

        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            var willDie = player.Health > 0 && player.Health - damage <= 0;

            player.TakeDamage(damage);
            if (willDie) player.CRpcDie();

            var owner = Utils.GetGameObjectFromConnection(connectionToClient);
            var ownerPlayer = owner.GetComponent<Player>();
        }
        Destroy();
    }

    [Server]
    private void Destroy()
    {
        NetworkServer.Destroy(gameObject);
        GameObject.Destroy(gameObject);
    }



}
