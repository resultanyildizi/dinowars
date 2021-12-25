using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DinowarsPlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private Player sanyaPlayerPrefab = null;
    [SerializeField] private Player uxgylPlayerPrefab = null;
    [SerializeField] private Player rextPlayerPrefab = null;
    [SerializeField] private Transform teamASpawnPoint;
    [SerializeField] private Transform teamBSpawnPoint;

    public override void OnStartServer()
    {
        DinowarsNetworkManager.OnServerReadied += SpawnPlayer;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        DinowarsNetworkManager.OnServerReadied -= SpawnPlayer;
    }


    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        var gamePlayer = conn.identity.gameObject.GetComponent<DinowarsNetworkGamePlayer>();

        Player playerInstance;
        Player playerPrefab = null;

        if (gamePlayer.Dino == DinowarsNetworkRoomPlayer.Dino.RexT)
            playerPrefab = rextPlayerPrefab;
        else if (gamePlayer.Dino == DinowarsNetworkRoomPlayer.Dino.Uxgyl)
            playerPrefab = uxgylPlayerPrefab;
        else if (gamePlayer.Dino == DinowarsNetworkRoomPlayer.Dino.Sanya)
            playerPrefab = sanyaPlayerPrefab;

        if(playerPrefab != null)
        {
            if(gamePlayer.Team == DinowarsNetworkRoomPlayer.Team.TeamA)
                playerInstance = Instantiate(playerPrefab, teamASpawnPoint.position, teamASpawnPoint.rotation);
            else
                playerInstance = Instantiate(playerPrefab, teamBSpawnPoint.position, teamBSpawnPoint.rotation);

            SetPlayer(playerInstance, gamePlayer);
            NetworkServer.Spawn(playerInstance.gameObject, conn);         
        }
    }

    private void SetPlayer(Player player, DinowarsNetworkGamePlayer gamePlayer)
    {
        Color blue, orange;
        ColorUtility.TryParseHtmlString("#6BBFFF", out blue);
        ColorUtility.TryParseHtmlString("#FFC75A", out orange);

        player.Team = gamePlayer.Team;
        player.PlayerName = gamePlayer.DisplayName;
   
        if (player.Team == DinowarsNetworkRoomPlayer.Team.TeamA)
            player.PlayerColor = blue;
        if (player.Team == DinowarsNetworkRoomPlayer.Team.TeamB)
            player.PlayerColor = orange;
    }


}
