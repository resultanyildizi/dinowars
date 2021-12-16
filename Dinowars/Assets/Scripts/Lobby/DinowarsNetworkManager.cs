using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections.Generic;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class DinowarsNetworkManager : NetworkManager
{
    [SerializeField] private int minPlayers = 2;
    [SerializeField] private string menuscene = string.Empty;
    [Header("Room")]
    [SerializeField] private DinowarsNetworkRoomPlayer roomPlayerPrefab;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action OnPlayersUpdated;

    public List<DinowarsNetworkRoomPlayer> RoomPlayers { get; } = new List<DinowarsNetworkRoomPlayer>();

    public override void OnStartServer()
    {
        Debug.Log("Server started");
    }

    public override void OnStartClient()
    {
        foreach (var prefab in spawnPrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect(); return;
        }

        if (!SceneManager.GetActiveScene().name.Equals(menuscene))
        {
            conn.Disconnect(); return;
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
        Debug.Log("Client is connected: " + conn.connectionId);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().name.Equals(menuscene))
        {
            DinowarsNetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsLeader = RoomPlayers.Count == 0;
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnClientDisconnected?.Invoke();
        Debug.Log("Client is disconnected: " + conn.connectionId);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            DinowarsNetworkRoomPlayer player = conn.identity.GetComponent<DinowarsNetworkRoomPlayer>();
            RoomPlayers.Remove(player);

            // NotifyPlayersReadyState
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }

    public void NotifyPlayersReadyState()
    {
        foreach(var player in RoomPlayers) {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    public bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) return false;

        foreach (var player in RoomPlayers) {
            if (!player.IsReady) return false;
        }

        return true;
    }


}
