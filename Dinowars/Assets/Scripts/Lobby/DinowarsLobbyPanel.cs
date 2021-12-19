using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class DinowarsLobbyPanel : MonoBehaviour
{
    private DinowarsNetworkRoomPlayer roomPlayer = null;


    [SerializeField] private TeamPlayerCard[] teamARoomPlayers;
    [SerializeField] private TeamPlayerCard[] teamBRoomPlayers;

    [Header("UI")]
    [SerializeField] private Button teamASelectButton;
    [SerializeField] private Button teamBSelectButton;
    [SerializeField] private Button toggleReadyButton;

    public DinowarsNetworkRoomPlayer RoomPlayer
    {
        get
        {
            if (roomPlayer == null)
                roomPlayer = DinowarsNetworkManager.Instance.GetAuthorizedPlayer();
            return roomPlayer;
        }
    }



    private void Awake() => DinowarsNetworkManager.OnPlayersUpdated += OnPlayersUpdated;

    private void OnPlayersUpdated()
    {
        ResetCards();

        for (int i = 0; i < DinowarsNetworkManager.Instance.TeamAPlayers.Count; i++)
            teamARoomPlayers[i].RoomPlayer = DinowarsNetworkManager.Instance.TeamAPlayers[i];

        for (int i = 0; i < DinowarsNetworkManager.Instance.TeamBPlayers.Count; i++)
            teamBRoomPlayers[i].RoomPlayer = DinowarsNetworkManager.Instance.TeamBPlayers[i];
    }

    private void ResetCards()
    {
        foreach (var card in teamARoomPlayers) card.RoomPlayer = null;
        foreach (var card in teamBRoomPlayers) card.RoomPlayer = null;
    }

    public void ChangeTeamToB()
    {
        RoomPlayer.CmdChangeTeam(DinowarsNetworkRoomPlayer.Team.TeamB);
    }

    public void ChangeTeamToA()
    {
        
        RoomPlayer.CmdChangeTeam(DinowarsNetworkRoomPlayer.Team.TeamA);
    }

    public void ToggleReady()
    {
        RoomPlayer.IsReady = !RoomPlayer.IsReady;
    }
}


