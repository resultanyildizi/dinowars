using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HealthKit : NetworkBehaviour
{
    [SerializeField]
    private double healingAmount;
    
    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Body") || collision.CompareTag("Foot"))
        {
            Player player = collision.GetComponentInParent<Player>();
            player.Heal(healingAmount);

            NetworkServer.Destroy(gameObject);
            GameObject.Destroy(gameObject);
        }
    }
}
