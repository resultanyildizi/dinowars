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
            PickedUp(player);
        }
    }
    [Command(requiresAuthority =false)]
    private void PickedUp(Player player)
    {   
        GameObject.Destroy(gameObject);
        ObjectSpawner.hkPickedUp(gameObject.GetInstanceID());
        player.Heal(healingAmount);
    }
}
