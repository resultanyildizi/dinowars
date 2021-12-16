using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private DinowarsNetworkManager dinowarsNetworkManager;
    [Header("Menu UI")]
    [SerializeField] private GameObject landingPanelUI;


    public void HostLobby()
    {
        dinowarsNetworkManager.StartHost();
        dinowarsNetworkManager.maxConnections = 5;
        landingPanelUI.SetActive(false);
    }
}
