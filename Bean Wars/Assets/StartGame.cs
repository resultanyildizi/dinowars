using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private SpriteRenderer startText;

    void Start()
    {
        startText = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        StartCoroutine(RemoveAfterSeconds(4, startText));

    }

    IEnumerator RemoveAfterSeconds(int seconds, SpriteRenderer obj)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(obj);
    }
}
