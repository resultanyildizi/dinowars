using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DinowarsNetworkRoomPlayer : NetworkBehaviour
{
    public enum Team { TeamA, TeamB, None }
    [SyncVar()]
    private bool isLeader = false;
    [SyncVar(hook = nameof(OnIsReadyChanged))]
    private bool isReady = false;
    [SyncVar(hook = nameof(OnPlayerTeamChanged))]
    private Team playerTeam = Team.None;
    [SyncVar(hook = nameof(OnDisplayNameChanged))]
    private string displayname = null;

    public Team PlayerTeam { get { return playerTeam; } set { playerTeam = value; } }
    public string DisplayName { get { return displayname; } set { displayname = value; } }
    public bool IsLeader { get { return isLeader; } set { isLeader = value; } }
    public bool IsReady { get { return isReady; } set { isReady = value; } }

    public event Action OnRoomPlayerChanged;

    public override void OnStartAuthority()
    {
        CmdChangeName(PlayerInputMenu.DisplayName);
    }

    public override void OnStartClient()
    {
        DinowarsNetworkManager.Instance.AddPlayerToTeam(this);
    }

    private void OnIsReadyChanged(bool oldValue, bool newValue)
    {
        OnRoomPlayerChanged?.Invoke();
        DinowarsNetworkManager.Instance.IsReadyToStart();
    }

    private void OnDisplayNameChanged(string oldValue, string newValue) => OnRoomPlayerChanged?.Invoke();

    private void OnPlayerTeamChanged(Team oldValue, Team newValue)
    {
        if (oldValue == Team.None) return;
        if (oldValue == newValue) return;

        if (newValue == Team.TeamA)
            DinowarsNetworkManager.Instance.ChangeTeamToA(this);
        else if (newValue == Team.TeamB)
            DinowarsNetworkManager.Instance.ChangeTeamToB(this);
    }

    [Command]
    public void CmdChangeName(string name) => DisplayName = name;
    
    [Command]
    public void CmdChangeTeam(Team team) => PlayerTeam = team;
    
    [Command]
    public void CmdChangeReady(bool ready) => IsReady = ready;
    
}
