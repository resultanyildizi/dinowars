using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class DinowarsNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        Debug.Log("Server started");
    }
    public override void OnStopServer()
    {
        Debug.Log("Server stopped");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Number of players:" + this.numPlayers);
        Debug.Log("Client is connected: " + conn.connectionId);
    }
}
