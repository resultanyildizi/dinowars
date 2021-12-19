using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeamPlayerCard : MonoBehaviour
{
    private DinowarsNetworkRoomPlayer roomPlayer;

    [Header("UI")]
    [SerializeField] private Text NameText;
    [SerializeField] private Text StatusText;


    public DinowarsNetworkRoomPlayer RoomPlayer {
        set
        {
            roomPlayer = value;
            if(value == null)
            {
                NameText.text = "Player Name";
                StatusText.text = "Waiting for player...";
                StatusText.color = Color.gray;
            } else
            {
                UpdateUI();
                roomPlayer.OnRoomPlayerChanged += UpdateUI;
            }
        }
    }

    private void UpdateUI()
    {
        if(roomPlayer != null)
        {
            NameText.text = roomPlayer.DisplayName;
            StatusText.text = roomPlayer.IsReady ? "Ready" : "Not ready";
            StatusText.color = roomPlayer.IsReady ? Color.green : Color.red;
        }
    }

    
}
