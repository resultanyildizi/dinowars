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
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gun.transform.position = new Vector3(rb.transform.position.x,
                                             rb.transform.position.y,
                                             rb.transform.position.z);
    }

    private void OnDestroy()
    {
        Object.Destroy(gun);
    }
}
