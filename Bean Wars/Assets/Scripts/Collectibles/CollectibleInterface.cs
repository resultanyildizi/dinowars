using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectibleInterface : MonoBehaviour
{

    [SerializeField]
    AnimationCurve curve;

    public abstract void PickedUp(Collider2D collision);

    public void Osciliate(Rigidbody2D body) {

        body.transform.position = new Vector2(body.transform.position.x, curve.Evaluate((Time.time % curve.length)));


    }




}
