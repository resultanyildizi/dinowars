using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : CollectibleInterface
{
    public static event System.Action<double> OnHealthKitDestroyedEvent;

    [SerializeField]
    private double healingAmount;
    public static bool kitJustPickedUp = false;
    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Osciliate(body);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }

    public override void PickedUp(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody"))
        {
            Destroy(gameObject);
            kitJustPickedUp = true;
            OnHealthKitDestroyedEvent?.Invoke(this.healingAmount);
        }
    }
}
