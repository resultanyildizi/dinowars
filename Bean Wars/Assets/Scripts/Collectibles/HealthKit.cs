using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    [SerializeField]
    private float healingAmount;
    
    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Body"))
        {
            Player player = collision.GetComponentInParent<Player>();
            PickedUp(player);
        }
    }

    private void PickedUp(Player player)
    {
        gameObject.SetActive(false);
        player.Heal(healingAmount);
    }
}
