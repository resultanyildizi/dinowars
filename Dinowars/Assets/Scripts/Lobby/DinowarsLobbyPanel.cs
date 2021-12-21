using UnityEngine;
using UnityEngine.UI;

public class DinowarsLobbyPanel : MonoBehaviour
{
    private DinowarsNetworkRoomPlayer roomPlayer = null;


    [SerializeField] private TeamPlayerCard[] teamARoomPlayers;
    [SerializeField] private TeamPlayerCard[] teamBRoomPlayers;

    [Header("UI")]
    [SerializeField] private Button toggleButton;
    [SerializeField] private Button startGameButton;

    public DinowarsNetworkRoomPlayer RoomPlayer
    {
        get
        {
            if (roomPlayer == null)
                roomPlayer = DinowarsNetworkManager.Instance.GetAuthorizedPlayer();
            return roomPlayer;
        }
    }

    private void Awake()
    {
        DinowarsNetworkManager.OnPlayersUpdated += OnPlayersUpdated;
        DinowarsNetworkManager.OnReadyStateChanged += OnReadyStateChanged;
    }

    private void OnReadyStateChanged(bool ready)
    {
        startGameButton.gameObject.SetActive(RoomPlayer.IsLeader && ready);
    }

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
        bool isReady = RoomPlayer.IsReady;
        RoomPlayer.CmdChangeReady(!isReady);
        if(!isReady)
        {
            toggleButton.GetComponentInChildren<Text>().text = "Ready";
            toggleButton.GetComponent<Image>().color = Color.green;
        } else
        {
            Color color;
            toggleButton.GetComponentInChildren<Text>().text = "Not Ready";
            ColorUtility.TryParseHtmlString("#6F153B", out color);
            toggleButton.GetComponent<Image>().color = color;
        }
    }
}


