using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lawa : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerGo = collision.gameObject;
            var player = playerGo.GetComponent<Player>();
            player.TakeDamage(100);
        }
    }
}
