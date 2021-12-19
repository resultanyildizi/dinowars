using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DinowarsNetworkRoomPlayer : NetworkBehaviour
{
    public enum Team { TeamA, TeamB, None }
    [SyncVar(hook = nameof(OnIsLeaderChanged))]
    private bool isLeader = false;
    [SyncVar(hook = nameof(OnIsReadyChanged))]
    private bool isReady = false;
    [SyncVar(hook = nameof(OnPlayerTeamChanged))]
    private Team playerTeam = Team.None;
    [SyncVar(hook = nameof(OnDisplayNameChanged))]
    private string displayname = null;

    public event Action OnDisplayNameChangedEvent;

    public Team PlayerTeam { get { return playerTeam; } set { playerTeam = value; } }
    public string DisplayName { get { return displayname; } set { displayname = value; } }
    public bool IsLeader { get { return isLeader; } set { isLeader = value; } }
    public bool IsReady { get { return isReady; } set { isReady = value; } }

    public override void OnStartAuthority()
    {
        CmdChangeName(PlayerInputMenu.DisplayName);
    }

    public override void OnStartClient()
    {
        DinowarsNetworkManager.Instance.AddPlayerToTeam(this);
    }

    public void HandleReadyToStart(bool isReadyToStart)
    {
        if (!isLeader) return;

    }


    private void OnIsLeaderChanged(bool oldValue, bool newValue) {}
    private void OnIsReadyChanged(bool oldValue, bool newValue) {}

    private void OnPlayerTeamChanged(Team oldValue, Team newValue) {
        if (oldValue == Team.None) return;
        if (oldValue == newValue) return;

        Debug.Log(oldValue + " - " + newValue);

        if (newValue == Team.TeamA)
            DinowarsNetworkManager.Instance.ChangeTeamToA(this);
        else if (newValue == Team.TeamB)
            DinowarsNetworkManager.Instance.ChangeTeamToB(this);
                
    }
    private void OnDisplayNameChanged(string oldValue, string newValue)
    {
        OnDisplayNameChangedEvent?.Invoke();
    }

    [Command]
    public void CmdChangeName(string name)
    {
        DisplayName = name;
    }

    [Command]
    public void CmdChangeTeam(Team team)
    {
        PlayerTeam = team;
    }
}
