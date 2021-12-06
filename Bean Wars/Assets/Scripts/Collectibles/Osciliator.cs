using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Osciliator : MonoBehaviour
{

    [SerializeField]
    AnimationCurve curve;
    Vector3 initialPosition;


    Rigidbody2D rigidbody2d;
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        initialPosition = rigidbody2d.transform.position;
    }

    private void Update()
    {
        transform.position = new Vector2(initialPosition.x, initialPosition.y + curve.Evaluate((Time.time % curve.length)));
    }

}
