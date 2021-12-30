using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDirectionController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnDirectionChanged)) ]
    int playerDirection = 1;

    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject hand;
    
    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    private void Update()
    {
        if (hand.transform.childCount == 0) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (mousePos.x - playerBody.transform.position.x >= 0)
            CmdChangeDirection(1);
        else
            CmdChangeDirection(-1);
    }

    [Command]
    private void CmdChangeDirection(int dir)
    {
        playerDirection = dir;
    }

    private void OnDirectionChanged(int oldV, int newV)
    {
        playerBody.transform.localScale = new Vector3(newV, 1, 1);
        hand.transform.localScale = new Vector3(newV, 1, 1);
        hand.transform.localPosition = new Vector3(Math.Abs(hand.transform.localPosition.x) * newV, hand.transform.localPosition.y, 1);
    }
}
