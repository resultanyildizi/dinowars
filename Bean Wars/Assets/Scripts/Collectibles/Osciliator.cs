using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Osciliator : MonoBehaviour
{

    [SerializeField]
    AnimationCurve curve;

    Rigidbody2D rigidbody;
    Vector3 initialPosition;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        initialPosition = rigidbody.transform.position;
    }

    private void Update()
    {
        Osciliate();
    }

    private void Osciliate() {

        transform.position = new Vector2(initialPosition.x, initialPosition.y + curve.Evaluate((Time.time % curve.length)));


    }




}
