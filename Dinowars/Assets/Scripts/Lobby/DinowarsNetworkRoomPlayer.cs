using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DinowarsNetworkRoomPlayer : NetworkBehaviour
{
    public bool IsLeader { get; set; } = false;
    public bool IsReady { get; set; } = false;

    public void HandleReadyToStart(bool isReadyToStart)
    {
        if (!IsLeader) return;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
