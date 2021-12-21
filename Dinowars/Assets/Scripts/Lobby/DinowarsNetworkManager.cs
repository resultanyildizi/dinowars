using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections.Generic;

public class DinowarsNetworkManager : NetworkManager
{
    public static DinowarsNetworkManager Instance { get { return NetworkManager.singleton as DinowarsNetworkManager; } }


    [SerializeField] private int minPlayers = 1;
    [SerializeField] private string menuscene = string.Empty;
    [Header("Room")]
    [SerializeField] private DinowarsNetworkRoomPlayer roomPlayerPrefab;


    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action OnPlayersUpdated;
    public static event Action<bool> OnReadyStateChanged;


    public List<DinowarsNetworkRoomPlayer> TeamBPlayers { get; } = new List<DinowarsNetworkRoomPlayer>();
    public List<DinowarsNetworkRoomPlayer> TeamAPlayers { get; } = new List<DinowarsNetworkRoomPlayer>();


    private int maxTeamAPlayerCount;
    private int maxTeamBPlayerCount;

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
            roomPlayerInstance.IsLeader = TeamAPlayers.Count == 0 && TeamBPlayers.Count == 0;
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
            if (player.PlayerTeam == DinowarsNetworkRoomPlayer.Team.TeamA)
                TeamAPlayers.Remove(player);
            else if (player.PlayerTeam == DinowarsNetworkRoomPlayer.Team.TeamB)
                TeamAPlayers.Remove(player);

            OnPlayersUpdated?.Invoke();
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        TeamAPlayers.Clear();
        TeamBPlayers.Clear();
        OnPlayersUpdated?.Invoke();
    }

  

    public void IsReadyToStart()
    {
        if (numPlayers < minPlayers)
        {
            OnReadyStateChanged?.Invoke(false);
            return;
        }

        foreach (var player in TeamAPlayers)
        {
            if (!player.IsReady)
            {
                OnReadyStateChanged?.Invoke(false);
                return;
            }
        }

        foreach (var player in TeamBPlayers)
        {
            if (!player.IsReady)
            {
                OnReadyStateChanged?.Invoke(false);
                return;
            }
        }

        OnReadyStateChanged?.Invoke(true);
    }

    public bool AddPlayerToTeam(DinowarsNetworkRoomPlayer player)
    {
        maxTeamAPlayerCount = (int)Math.Ceiling((decimal)maxConnections / 2);
        maxTeamBPlayerCount = maxConnections - maxTeamAPlayerCount;

        if (TeamAPlayers.Count < maxTeamAPlayerCount)
        {
            TeamAPlayers.Add(player);
            player.PlayerTeam = DinowarsNetworkRoomPlayer.Team.TeamA;
            OnPlayersUpdated?.Invoke();
            return true;
        }

        if (TeamBPlayers.Count < maxTeamBPlayerCount)
        {
            TeamBPlayers.Add(player);
            player.PlayerTeam = DinowarsNetworkRoomPlayer.Team.TeamB;
            OnPlayersUpdated?.Invoke();
            return true;
        }

        return false;
    }

    public void ChangeTeamToB(DinowarsNetworkRoomPlayer roomPlayer)
    {
        if (roomPlayer != null)
        {
            TeamAPlayers.Remove(roomPlayer);
            TeamBPlayers.Add(roomPlayer);
            OnPlayersUpdated?.Invoke();
        }
    }

    public void ChangeTeamToA(DinowarsNetworkRoomPlayer roomPlayer)
    {
        if (roomPlayer != null)
        {
            TeamBPlayers.Remove(roomPlayer);
            TeamAPlayers.Add(roomPlayer);
            OnPlayersUpdated?.Invoke();
        }
    }

    public DinowarsNetworkRoomPlayer GetAuthorizedPlayer()
    {
        foreach (var player in TeamBPlayers)
            if (player.hasAuthority) return player;

        foreach (var player in TeamAPlayers)
            if (player.hasAuthority) return player;

        return null;
    }
}

