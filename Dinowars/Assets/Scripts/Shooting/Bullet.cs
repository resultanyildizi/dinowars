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
            var ownerGo = GetGameObjectFromConnection();
            if(ownerGo != null)
            {
                var ownerPlayer = ownerGo.GetComponent<Player>();
                var playerGo = collision.gameObject;
                var player = playerGo.GetComponent<Player>();

                if(!ownerPlayer.Team.Equals(player.Team))
                    player.TakeDamage(damage);

                Debug.Log(ownerPlayer.PlayerName + " damaged " + player.PlayerName + " by " + damage);
            }
        }
        Destroy();
    }

    [Server]
    private void Destroy()
    {
        NetworkServer.Destroy(this.gameObject);
        GameObject.Destroy(this.gameObject);
    }

    private GameObject GetGameObjectFromConnection()
    {
        var allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (var p in allPlayers)
        {
            var netId = p.GetComponent<NetworkIdentity>();
            if (netId != null && netId.connectionToClient == this.connectionToClient)
                return p;

        }
        return null;
    }



}
