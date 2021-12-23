using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DinowarsNetworkGamePlayer : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Loading...";
    [SyncVar]
    private DinowarsNetworkRoomPlayer.Dino dino = DinowarsNetworkRoomPlayer.Dino.None;
    [SyncVar]
    private DinowarsNetworkRoomPlayer.Team team = DinowarsNetworkRoomPlayer.Team.None;

    public DinowarsNetworkRoomPlayer.Team Team { get => team; set => team = value; }
    public DinowarsNetworkRoomPlayer.Dino Dino { get => dino; set => dino = value; }
    public string DisplayName { get => displayName; set => displayName = value; }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);
        DinowarsNetworkManager.Instance.AddGamePlayer(this);
    }

    [Server]
    public void SetPlayer(string displayName, DinowarsNetworkRoomPlayer.Team team, DinowarsNetworkRoomPlayer.Dino dino)
    {
        Debug.Log("Setting player");
        this.displayName = displayName;
        this.team = team;
        this.dino = dino;
    }
}
