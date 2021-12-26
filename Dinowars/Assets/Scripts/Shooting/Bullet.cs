using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private double damage = 30;

    public GameObject shooter;
    private GameObject[] players;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (collision.gameObject.CompareTag("Player"))
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject plyr in players)
            {


                if (plyr.GetComponent<NetworkIdentity>().connectionToClient == this.connectionToClient)
                {
                    shooter = plyr;
                    Debug.Log(plyr.GetComponent<Player>().playerName);
                }
            }
            if (player.team == shooter.GetComponent<Player>().team)
            {
                Destroy();
                return;
            }
            player.TakeDamage(damage);
            if (player.health <= 0)
            {   //ölsün










                var killerName = shooter.GetComponent<Player>().playerName;

                string killedName = player.GetComponent<Player>().playerName;
                Debug.Log(string.Format("{0} Has killed {1}", killerName, killedName));

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


}
