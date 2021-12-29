using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public static class Utils
{
    public static DinowarsNetworkGamePlayer GetGameObjectFromConnection(NetworkConnection conn)
    {
        var allPlayers = GameObject.FindGameObjectsWithTag("GamePlayer");

        foreach (var p in allPlayers)
        {
            var netId = p.GetComponent<NetworkIdentity>();
            if (netId != null && netId.connectionToClient == conn)
                return p.GetComponent<DinowarsNetworkGamePlayer>();
        }
        return null;
    }
}
