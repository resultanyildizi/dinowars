using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections.Generic;
using System.Collections;
using static DinowarsGameResult;

public class DinowarsNetworkManager : NetworkManager
{
    public static DinowarsNetworkManager Instance { get { return NetworkManager.singleton as DinowarsNetworkManager; } }


    [SerializeField] private int minPlayers = 1;
    [SerializeField] private string menuscene = string.Empty;
    [Header("Room")]
    [SerializeField] private DinowarsNetworkRoomPlayer roomPlayerPrefab;
    [SerializeField] private DinowarsNetworkGamePlayer gamePlayerPrefab;
    [SerializeField] private DinowarsPlayerSpawnSystem spawnSystemPrefab;
    [SerializeField] private DinowarsGameResult gameResultPrefab;

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

    public string RoomName { get => roomName; set => roomName = value; }
    public string RoomDesc { get => roomDesc; set => roomDesc = value; }
    public int MapIndexValue { get => mapIndexValue; set => mapIndexValue = value; }
    public int GameTime { get => gameTime; set => gameTime = value; }
    public string IpAddress { get => ipAddress; set => ipAddress = value; }

    private String ipAddress;
    private int maxTeamAPlayerCount;
    private int maxTeamBPlayerCount;
    private int gameTime;
    private int currentTime;
    private int mapIndexValue;
    private String roomName;
    private String roomDesc;

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
            FindObjectOfType<AudioController>().Stop("MenuTheme");
            //FindObjectOfType<AudioController>().Play("BackgroundSound");
            for (int i = TeamARoomPlayers.Count - 1; i >= 0; i--)
                RoomPlayerToGamePlayer(TeamARoomPlayers[i]);
            for (int i = TeamBRoomPlayers.Count - 1; i >= 0; i--)
                RoomPlayerToGamePlayer(TeamBRoomPlayers[i]);
        } else if(SceneManager.GetActiveScene().name.Equals(getSceneName()) && newSceneName.StartsWith("LastScene"))
        {
            for (int i = TeamAGamePlayers.Count - 1; i >= 0; i--)
                GamePlayerToGameResult(TeamAGamePlayers[i]);
            for (int i = TeamBGamePlayers.Count - 1; i >= 0; i--)
                GamePlayerToGameResult(TeamBGamePlayers[i]);
        }
        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.Equals(getSceneName()))
        {
            GameObject playerSpawnSystemInstance = Instantiate(spawnSystemPrefab.gameObject, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(playerSpawnSystemInstance);

            GameObject objectSpawner = Instantiate(healthkitSpawnerPrefab.gameObject, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(objectSpawner);

            GameObject rifleSpawner = Instantiate(rifleSpawnerPrefab.gameObject, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(rifleSpawner);

            currentTime = gameTime * 60;
            StartCoroutine(StartTimer());
        }
    }

    private void RoomPlayerToGamePlayer(DinowarsNetworkRoomPlayer player)
    {
        var conn = player.connectionToClient;
        var gamePlayerInstance = Instantiate(gamePlayerPrefab);
        gamePlayerInstance.SetPlayer(player.DisplayName, player.PlayerTeam, player.PlayerDino);
        NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
    }

    private void GamePlayerToGameResult(DinowarsNetworkGamePlayer player)
    {
        var conn = player.connectionToClient;
        var gameResultInstance = Instantiate(gameResultPrefab);

        var teamAScore = calculateTeamScore(DinowarsNetworkRoomPlayer.Team.TeamA);
        var teamBScore = calculateTeamScore(DinowarsNetworkRoomPlayer.Team.TeamB);

        var winner = calculateWinner();

        var gameResult = winner == DinowarsNetworkRoomPlayer.Team.None ? Result.DRAW:
                                    player.Team == winner ? Result.WIN : Result.LOSE;

        gameResultInstance.SetPlayer(gameResult, teamAScore, teamBScore);
        
        NetworkServer.ReplacePlayerForConnection(conn, gameResultInstance.gameObject);
        NetworkServer.Destroy(player.gameObject);
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
            ServerChangeScene(getSceneName());
    }

    public void AddGamePlayer(DinowarsNetworkGamePlayer player)
    {
        if (player.Team == DinowarsNetworkRoomPlayer.Team.TeamA)
            TeamAGamePlayers.Add(player);
        else if (player.Team == DinowarsNetworkRoomPlayer.Team.TeamB)
            TeamBGamePlayers.Add(player);
    }

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

    private String getSceneName()
    {
        if (MapIndexValue == 0)
        {
            return "CaveScene";
        }
        else if (MapIndexValue == 1)
        {
            return "ForestScene";
        }
        else
        {
            return "CaveScene";
        }
    }

    IEnumerator StartTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            currentTime -= 1;

            foreach (var gp in TeamAGamePlayers)
                gp.GameTime = currentTime;

            foreach (var gp in TeamBGamePlayers)
                gp.GameTime = currentTime;

            if (currentTime <= 0)
                break;
        }

        ServerChangeScene("LastScene");
    }

    private DinowarsNetworkRoomPlayer.Team calculateWinner()
    {
        var teamAScore = calculateTeamScore(DinowarsNetworkRoomPlayer.Team.TeamA);
        var teamBScore = calculateTeamScore(DinowarsNetworkRoomPlayer.Team.TeamB);

       if(teamAScore > teamBScore)
            return DinowarsNetworkRoomPlayer.Team.TeamA;
       else if(teamAScore < teamBScore)
            return DinowarsNetworkRoomPlayer.Team.TeamB;
       else
            return DinowarsNetworkRoomPlayer.Team.None;
    }

    private int calculateTeamScore(DinowarsNetworkRoomPlayer.Team team)
    {
        if (team == DinowarsNetworkRoomPlayer.Team.None) return -999;

        List<DinowarsNetworkGamePlayer> gamePlayers;

        if (team == DinowarsNetworkRoomPlayer.Team.TeamA)
            gamePlayers = TeamAGamePlayers;
        else
            gamePlayers = TeamBGamePlayers;

        int score = 0;
        foreach(var gp in gamePlayers)
            score += gp.Killed * 10 - gp.Death * 5;
        

        return score;
    }



}

