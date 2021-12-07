using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManagerLobby;
    [Header("Menu UI")]
    [SerializeField] private GameObject landingPanelUI;


    public void HostLobby()
    {
        networkManagerLobby?.StartHost();
        landingPanelUI.SetActive(false);
    }
}
