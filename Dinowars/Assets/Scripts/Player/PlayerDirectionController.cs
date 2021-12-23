using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDirectionController : NetworkBehaviour
{
    [SerializeField] private GameObject playerBody;

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (mousePos.x - playerBody.transform.position.x >= 0)
            playerBody.transform.localScale = new Vector3(1, 1, 1);
        else
            playerBody.transform.localScale = new Vector3(-1, 1, 1);
    }
}
