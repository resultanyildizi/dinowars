using System;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private DinowarsNetworkGamePlayer gamePlayerPrefab;
    [SerializeField] private DinowarsPlayerSpawnSystem cavePlayerSpawnSystemPrefab;

    [SerializeField] private ObjectSpawner healthkitSpawnerPrefab;
    [SerializeField] private ObjectSpawner rifleSpawnerPrefab;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action OnPlayersUpdated;
    public static event Action<bool> OnReadyStateChanged;
    public static event Action<NetworkConnection> OnServerReadied;



    public List<DinowarsNetworkRoomPlayer> TeamBRoomPlayers { get; } = new List<DinowarsNetworkRoomPlayer>();
    public List<DinowarsNetworkRoomPlayer> TeamARoomPlayers { get; } = new List<DinowarsNetworkRoomPlayer>();


    public List<DinowarsNetworkGamePlayer> TeamBGamePlayers { get; } = new List<DinowarsNetworkGamePlayer>();
    public List<DinowarsNetworkGamePlayer> TeamAGamePlayers { get; } = new List<DinowarsNetworkGamePlayer>();

    // public Dictionary<DinowarsNetworkGamePlayer, Tuple<int, int>> TeamAScores = new Dictionary<DinowarsNetworkGamePlayer, Tuple<int, int>>();
    // public Dictionary<DinowarsNetworkGamePlayer, Tuple<int, int>> TeamBScores = new Dictionary<DinowarsNetworkGamePlayer, Tuple<int, int>>();


    private int maxTeamAPlayerCount;
    private int maxTeamBPlayerCount;
    public int timeValue;
    public int roundValue;
    public int modeIndexValue;
    public int mapIndexValue;
    public String roomName;
    public String roomDesc;

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
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().name.Equals(menuscene))
        {
            DinowarsNetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsLeader = TeamARoomPlayers.Count == 0 && TeamBRoomPlayers.Count == 0;
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            DinowarsNetworkRoomPlayer player = conn.identity.GetComponent<DinowarsNetworkRoomPlayer>();
            if (player.PlayerTeam == DinowarsNetworkRoomPlayer.Team.TeamA)
                TeamARoomPlayers.Remove(player);
            else if (player.PlayerTeam == DinowarsNetworkRoomPlayer.Team.TeamB)
                TeamBRoomPlayers.Remove(player);

            OnPlayersUpdated?.Invoke();
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        TeamARoomPlayers.Clear();
        TeamBRoomPlayers.Clear();
        TeamAGamePlayers.Clear();
        TeamBGamePlayers.Clear();
        OnPlayersUpdated?.Invoke();
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (SceneManager.GetActiveScene().name.Equals(menuscene) && newSceneName.StartsWith(getSceneName()))
        {
            for (int i = TeamARoomPlayers.Count - 1; i >= 0; i--)
            {
                RoomPlayerToGamePlayer(TeamARoomPlayers[i]);
            }
            for (int i = TeamBRoomPlayers.Count - 1; i >= 0; i--)
            {
                RoomPlayerToGamePlayer(TeamBRoomPlayers[i]);
            }
        }
        base.ServerChangeScene(newSceneName);
    }


    public override void OnClientSceneChanged(NetworkConnection conn)
    {

        base.OnClientSceneChanged(conn);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.Equals(getSceneName()))
        {
            GameObject playerSpawnSystemInstance = Instantiate(cavePlayerSpawnSystemPrefab.gameObject, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(playerSpawnSystemInstance);

            GameObject objectSpawner = Instantiate(healthkitSpawnerPrefab.gameObject, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(objectSpawner);

            GameObject rifleSpawner = Instantiate(rifleSpawnerPrefab.gameObject, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(rifleSpawner);
        }
    }

    private void RoomPlayerToGamePlayer(DinowarsNetworkRoomPlayer player)
    {

        var conn = player.connectionToClient;
        var gamePlayerInstance = Instantiate(gamePlayerPrefab);

        gamePlayerInstance.SetPlayer(player.DisplayName, player.PlayerTeam, player.PlayerDino);

        NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
    }

    public void IsReadyToStart()
    {
        if (numPlayers < minPlayers || TeamARoomPlayers.Count == 0 || TeamBRoomPlayers.Count == 0)
        {
            OnReadyStateChanged?.Invoke(false);
            return;
        }

        foreach (var player in TeamARoomPlayers)
        {
            if (!player.IsReady)
            {
                OnReadyStateChanged?.Invoke(false);
                return;
            }
        }

        foreach (var player in TeamBRoomPlayers)
        {
            if (!player.IsReady)
            {
                OnReadyStateChanged?.Invoke(false);
                return;
            }
        }

        OnReadyStateChanged?.Invoke(true);
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name.Equals(menuscene))
        {
            ServerChangeScene(getSceneName());
        }
    }

    public void AddGamePlayer(DinowarsNetworkGamePlayer player)
    {
        if (player.Team == DinowarsNetworkRoomPlayer.Team.TeamA)
            TeamAGamePlayers.Add(player);
        else if (player.Team == DinowarsNetworkRoomPlayer.Team.TeamB)
            TeamBGamePlayers.Add(player);
    }

    // public void InitPlayerScore(DinowarsNetworkGamePlayer player)
    // {
    //     if (player.Team == DinowarsNetworkRoomPlayer.Team.TeamA)
    //         TeamAScores[player] = new Tuple<int, int>(0, 0);
    //     else if (player.Team == DinowarsNetworkRoomPlayer.Team.TeamB)
    //         TeamBScores[player] = new Tuple<int, int>(0, 0);
    // }

    public bool AddPlayerToTeam(DinowarsNetworkRoomPlayer player)
    {
        switch (player.PlayerTeam)
        {
            case DinowarsNetworkRoomPlayer.Team.TeamA:
                TeamARoomPlayers.Add(player);
                OnPlayersUpdated?.Invoke();
                return true;
            case DinowarsNetworkRoomPlayer.Team.TeamB:
                TeamBRoomPlayers.Add(player);
                OnPlayersUpdated?.Invoke();
                return true;
            case DinowarsNetworkRoomPlayer.Team.None:
                maxTeamAPlayerCount = (int)Math.Ceiling((decimal)maxConnections / 2);
                maxTeamBPlayerCount = maxConnections - maxTeamAPlayerCount;
                if (TeamARoomPlayers.Count < maxTeamAPlayerCount)
                {
                    player.PlayerTeam = DinowarsNetworkRoomPlayer.Team.TeamA;
                    TeamARoomPlayers.Add(player);
                    OnPlayersUpdated?.Invoke();
                    return true;
                }

                if (TeamBRoomPlayers.Count < maxTeamBPlayerCount)
                {
                    player.PlayerTeam = DinowarsNetworkRoomPlayer.Team.TeamB;
                    TeamBRoomPlayers.Add(player);
                    OnPlayersUpdated?.Invoke();
                    return true;
                }
                return false;
            default: return false;
        }
    }

    public void ChangeTeamToB(DinowarsNetworkRoomPlayer roomPlayer)
    {
        if (roomPlayer != null)
        {
            TeamARoomPlayers.Remove(roomPlayer);
            TeamBRoomPlayers.Add(roomPlayer);
            OnPlayersUpdated?.Invoke();
        }
    }

    public void ChangeTeamToA(DinowarsNetworkRoomPlayer roomPlayer)
    {
        if (roomPlayer != null)
        {
            TeamBRoomPlayers.Remove(roomPlayer);
            TeamARoomPlayers.Add(roomPlayer);
            OnPlayersUpdated?.Invoke();
        }
    }

    public DinowarsNetworkRoomPlayer GetAuthorizedPlayer()
    {
        foreach (var player in TeamBRoomPlayers)
            if (player.hasAuthority) return player;

        foreach (var player in TeamARoomPlayers)
            if (player.hasAuthority) return player;

        return null;
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);
    }

    // public void PlayerKillsPlayer(DinowarsNetworkGamePlayer killed, DinowarsNetworkGamePlayer killer)
    // {
    //     Debug.Log("Calculating new scores");
    //     if (killed.Team == DinowarsNetworkRoomPlayer.Team.TeamA && killer.Team == DinowarsNetworkRoomPlayer.Team.TeamB)
    //     {
    //         var killedScr = TeamAScores[killed];
    //         var killerScr = TeamBScores[killer];

    //         TeamAScores[killed] = new Tuple<int, int>(killedScr.Item1, killedScr.Item2 + 1);
    //         TeamBScores[killer] = new Tuple<int, int>(killerScr.Item1 + 1, killerScr.Item2);
    //         return;
    //     }

    //     if (killed.Team == DinowarsNetworkRoomPlayer.Team.TeamB && killer.Team == DinowarsNetworkRoomPlayer.Team.TeamA)
    //     {
    //         var killedScr = TeamBScores[killed];
    //         var killerScr = TeamAScores[killer];

    //         TeamBScores[killed] = new Tuple<int, int>(killedScr.Item1, killedScr.Item2 + 1);
    //         TeamAScores[killer] = new Tuple<int, int>(killerScr.Item1 + 1, killerScr.Item2);
    //         return;
    //     }
    // }



    private String getSceneName()
    {
        if (mapIndexValue == 0)
        {
            return "CaveScene";
        }
        else if (mapIndexValue == 1)
        {
            return "ForestScene";
        }
        else
        {
            return "CaveScene";
        }
    }


}

