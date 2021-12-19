using System.Collections;
using System.Collections.Generic;
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
            teamARoomPlayers[i].RoomPlayer = DinowarsNetworkManager.Instance.TeamAPlayers[i];
        }

        for (int i = 0; i < DinowarsNetworkManager.Instance.TeamBPlayers.Count; i++)
        {
            teamBRoomPlayers[i].RoomPlayer = DinowarsNetworkManager.Instance.TeamAPlayers[i];
        }
    }

    private void ResetCards()
    {
        foreach(var card in teamARoomPlayers)
        {
            card.RoomPlayer = null;
        }
        foreach(var card in teamBRoomPlayers)
        {
            card.RoomPlayer = null;
        }

        Debug.Log("Reset All CARDS");
    }

    public void ChangeTeamToB()
    {
        DinowarsNetworkRoomPlayer roomPlayer = null;
        for (int i = DinowarsNetworkManager.Instance.TeamAPlayers.Count - 1; i >= 0; i--)
        {
            roomPlayer = DinowarsNetworkManager.Instance.TeamAPlayers[i];
            if (roomPlayer.hasAuthority) break;
        }

        if (roomPlayer != null)
        {
            roomPlayer.PlayerTeam = DinowarsNetworkRoomPlayer.Team.TeamB;
            DinowarsNetworkManager.Instance.TeamAPlayers.Remove(roomPlayer);
            DinowarsNetworkManager.Instance.TeamBPlayers.Add(roomPlayer);
            OnPlayersUpdated();
        }
    }
}
