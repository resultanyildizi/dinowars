using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeamPlayerCard : MonoBehaviour
{
    private DinowarsNetworkRoomPlayer roomPlayer;

    [Header("UI")]
    [SerializeField] private Text NameText;
    [SerializeField] private Text StatusText;
    [SerializeField] private Text DinoNameText;


    public DinowarsNetworkRoomPlayer RoomPlayer {
        set
        {
            roomPlayer = value;
            if(value == null)
            {
                NameText.text = "Player Name";
                StatusText.text = "Waiting for player...";
                StatusText.color = Color.gray;
                DinoNameText.text = "";
            }
            else
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
            DinoNameText.text = GetDinoName(roomPlayer);
            DinoNameText.color = GetDinoColor(roomPlayer);
        }
    }

    private string GetDinoName(DinowarsNetworkRoomPlayer roomPlayer)
    {

        switch (roomPlayer.PlayerDino)
        {
            case Dino.RexT:
                return "Rext";
            case Dino.Uxgyl:
                return "Uxgyl";
            case Dino.Sanya:
                return "Sanya";
            default:
                return "";
        }
    }

    private Color GetDinoColor(DinowarsNetworkRoomPlayer roomPlayer)
    {
        Color color;

        switch (roomPlayer.PlayerDino)
        {
            case Dino.RexT:
                ColorUtility.TryParseHtmlString("#EC6339", out color);
                return color;
            case Dino.Uxgyl:
                ColorUtility.TryParseHtmlString("#56BCA6", out color);
                return color;
            case Dino.Sanya:
                ColorUtility.TryParseHtmlString("#DF6B92", out color);
                return color;
            default:
                ColorUtility.TryParseHtmlString("#FFFFFF", out color);
                return color;
        }

    }

}
