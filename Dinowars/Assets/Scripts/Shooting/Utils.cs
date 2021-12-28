using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public static class Utils
{
    public static GameObject GetGameObjectFromConnection(NetworkConnection conn)
    {
        var allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach(var p in allPlayers)
        {
            var netId = p.GetComponent<NetworkIdentity>();
           if (netId != null && netId.connectionToClient == conn) 
                return p;
           
        }
        return null;
    }


}
