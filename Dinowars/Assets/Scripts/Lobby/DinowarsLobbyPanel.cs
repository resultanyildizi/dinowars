using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class DinowarsLobbyPanel : MonoBehaviour
{
    [SerializeField] TeamPlayerCard[] teamARoomPlayers;
    [SerializeField] TeamPlayerCard[] teamBRoomPlayers;

    [SerializeField] Button teamASelectButton;
    [SerializeField] Button teamBSelectButton;

    private void OnEnable()
    {
        Debug.Log("Hello i am enabled");
        DinowarsNetworkManager.OnPlayersUpdated += OnPlayersUpdated;
    }


    private void OnPlayersUpdated()
    {
        ResetCards();

        for (int i = 0; i < DinowarsNetworkManager.Instance.TeamAPlayers.Count; i++)
        {
            Debug.Log("A: " + i);
            teamARoomPlayers[i].RoomPlayer = DinowarsNetworkManager.Instance.TeamAPlayers[i];
        }

        for (int i = 0; i < DinowarsNetworkManager.Instance.TeamBPlayers.Count; i++)
        {
            Debug.Log("B: " + i);
            teamBRoomPlayers[i].RoomPlayer = DinowarsNetworkManager.Instance.TeamBPlayers[i];
        }
    }

    private void ResetCards()
    {
        foreach (var card in teamARoomPlayers)
        {
            card.RoomPlayer = null;
        }
        foreach (var card in teamBRoomPlayers)
        {
            card.RoomPlayer = null;
        }

        Debug.Log("Reset All CARDS");
    }

    public void ChangeTeamToB()
    {
        foreach (var player in DinowarsNetworkManager.Instance.TeamAPlayers)
        {
            if (player.hasAuthority)
            {
                Debug.Log(player);
                player.CmdChangeTeam(DinowarsNetworkRoomPlayer.Team.TeamB);
                return;
            }       
        }
    }

    public void ChangeTeamToA()
    {
        foreach (var player in DinowarsNetworkManager.Instance.TeamBPlayers)
        {
            if (player.hasAuthority)
            {
                Debug.Log(player);
                player.CmdChangeTeam(DinowarsNetworkRoomPlayer.Team.TeamA);
                return;
            }
        }
    }
}


