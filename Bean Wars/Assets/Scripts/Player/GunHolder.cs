using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunHolder : MonoBehaviour
{
    [SerializeField] GameObject gunPrefab;

    private GameObject gun;
    private Rigidbody2D rb;

    private float gunOffsetX;
    private float gunOffsetY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gun = Instantiate<GameObject>(gunPrefab, rb.transform.position, Quaternion.identity);
    }
    void Start()
    {
        gun.transform.SetParent(parent: this.transform, worldPositionStays: true);
    }

    private void OnDestroy()
    {
        Object.Destroy(gun);
    }
}
