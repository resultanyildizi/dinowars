using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(this.connectionToClient.identity.netId);
        Destroy();
    }

    [Server]
    private void Destroy()
    {
        NetworkServer.Destroy(this.gameObject);
        GameObject.Destroy(this.gameObject);
    }


}
