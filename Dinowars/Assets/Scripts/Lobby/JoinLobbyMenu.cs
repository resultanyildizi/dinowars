//using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private DinowarsNetworkManager dinowarsNetworkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPanelUI = null;
    [SerializeField] private GameObject loobyPanelUI = null;
    [SerializeField] private InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        ipAddressInputField.text = "localhost";
        DinowarsNetworkManager.OnClientConnected += HandleClientConnected;
        DinowarsNetworkManager.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        DinowarsNetworkManager.OnClientConnected -= HandleClientConnected;
        DinowarsNetworkManager.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;

        Debug.Log(ipAddress);

        dinowarsNetworkManager.networkAddress = ipAddress;
        dinowarsNetworkManager.StartClient();
        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);

        landingPanelUI.SetActive(false);
        loobyPanelUI.SetActive(true);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
