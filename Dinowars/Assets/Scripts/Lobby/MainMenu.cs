using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Menu UI")]
    [SerializeField] private GameObject landingPanelUI;
    [SerializeField] private GameObject lobbyPanelUI;

    public void HostLobby()
    {
        DinowarsNetworkManager.Instance.StartHost();
        DinowarsNetworkManager.Instance.maxConnections = 5;
        landingPanelUI.SetActive(false);
        lobbyPanelUI.SetActive(true);
    }
}
