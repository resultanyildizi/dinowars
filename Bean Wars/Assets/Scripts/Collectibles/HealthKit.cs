using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    public static event System.Action<double> OnHealthKitDestroyedEvent;

    [SerializeField]
    private double healingAmount;
    public static bool kitJustPickedUp = false;
    private Rigidbody2D body;

    private void Awake()
    {
        body = new Rigidbody2D();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBody"))
        {
            Destroy(gameObject);
            kitJustPickedUp = true;

            OnHealthKitDestroyedEvent?.Invoke(this.healingAmount);
        }
    }

}
