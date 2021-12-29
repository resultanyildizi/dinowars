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
            var ownerGp = Utils.GetGameObjectFromConnection(connectionToClient);

            if (ownerGp != null)
            {
                var player = collision.gameObject.GetComponent<Player>();
                var playerGp = Utils.GetGameObjectFromConnection(player.connectionToClient);

                var willDie = player.Health > 0 && player.Health - damage <= 0;

                if (!ownerGp.Team.Equals(player.Team))
                {
                    player.TakeDamage(damage);
                    if (willDie)
                    {
                        playerGp.IncreaseDeath();
                        ownerGp.IncreaseKill();
                    }

                }


            }
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
