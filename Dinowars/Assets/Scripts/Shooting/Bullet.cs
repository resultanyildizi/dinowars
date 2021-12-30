using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private double damage = 30;

    [SerializeField] private List<string> playerTags;
    [SerializeField] private List<string> ingoredTags;
    [SerializeField] private List<float> multipliers;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var tag = collider.gameObject.tag;
        if (ingoredTags.Contains(tag)) return;

        var isPlayer = playerTags.Contains(tag);
        var mutex = true;

        if (isPlayer && mutex)
        {
            mutex = false;
            var multiplier = multipliers[playerTags.IndexOf(tag)];
            var realDamage = damage * multiplier;
            
            var ownerGp = Utils.GetGameObjectFromConnection(connectionToClient);
            if (ownerGp != null)
            {
                var player = collider.transform.parent.parent.GetComponent<Player>();
                var playerGp = Utils.GetGameObjectFromConnection(player.connectionToClient);

                var willDie = player.Health > 0 && player.Health - realDamage <= 0;

                if (!ownerGp.Team.Equals(player.Team))
                {
                    player.TakeDamage(realDamage);
                    if (willDie)
                    {
                        playerGp.IncreaseDeath();
                        ownerGp.IncreaseKill();
                    }
                }
            }
        }
            DestroyBullet();
    }

    [Server]
    private void DestroyBullet()
    {
        GameObject.Destroy(gameObject);
        NetworkServer.Destroy(gameObject);
    }
}
