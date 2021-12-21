using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DinowarsNetworkGamePlayer : NetworkBehaviour
{
    private string displayName = "Loading...";
    private DinowarsNetworkRoomPlayer.Dino dino = DinowarsNetworkRoomPlayer.Dino.None;
    private DinowarsNetworkRoomPlayer.Team team = DinowarsNetworkRoomPlayer.Team.None;

    public DinowarsNetworkRoomPlayer.Team Team { get => team; }
    public DinowarsNetworkRoomPlayer.Dino Dino { get => dino; }
    public string DisplayName { get => displayName; }

    public override void OnStartClient()
    {
        DinowarsNetworkManager.Instance.AddGamePlayer(this);
        base.OnStartClient();
    }

    [Server]
    public void SetPlayer(string name, DinowarsNetworkRoomPlayer.Team team, DinowarsNetworkRoomPlayer.Dino dino)
    {
        this.name = name;
        this.team = team;
        this.dino = dino;
    }
}
