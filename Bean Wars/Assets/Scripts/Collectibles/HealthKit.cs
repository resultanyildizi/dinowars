using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    public static event System.Action<double> OnHealthKitDestroyedEvent;

    [SerializeField]
    private double healingAmount;
    
    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }

    public void PickedUp(Collider2D collision)
    {
        if (collision.CompareTag("Body"))
        {
            gameObject.SetActive(false);
            OnHealthKitDestroyedEvent?.Invoke(this.healingAmount);
        }
    }
}
