using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    [SerializeField]
    private PlayerCanvas playerCanvas;

    //private SpriteRenderer playerSprite;    
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        player.OnNameChangedEvent += SetPlayerNameText;
        player.OnColorChangedEvent+= SetPlayerColor;
    }

    private void SetPlayerNameText(string name)
    {
        if (player.isLocalPlayer) playerCanvas.PlayerNameText.text = name;
    }

    private void SetPlayerColor(Color color)
    {
        if (player.isLocalPlayer) {
            playerCanvas.PlayerNameText.color = color;
            //playerSprite.color = color;
        }
    }
}
