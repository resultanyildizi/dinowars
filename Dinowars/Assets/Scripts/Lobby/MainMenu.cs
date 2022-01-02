using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu UI")]
    [SerializeField] private GameObject landingPanelUI;
    [SerializeField] private GameObject lobbyPanelUI;
    [SerializeField] private GameObject settingPanelUI;

    [Header("Settings")]
    [SerializeField] Text timeTextValue = null;
    [SerializeField] Slider timeSlider = null;
    [SerializeField] Text maxPlayerTextValue = null;
    [SerializeField] Slider maxPlayerSlider = null;
    [SerializeField] InputField roomNameField = null;
    [SerializeField] InputField roomDescField = null;
    [SerializeField] Dropdown mapDropdown = null;
    [SerializeField] Sprite forestMap = null;
    [SerializeField] Sprite caveMap = null;
    [SerializeField] Text errorText = null;
    [SerializeField] GameObject errorTextPanel = null;


    private void Update()
    {
        if(mapDropdown != null)
        {
            if (mapDropdown.value == 0)
                settingPanelUI.GetComponent<Image>().sprite = caveMap;
            else if (mapDropdown.value == 1)
                settingPanelUI.GetComponent<Image>().sprite = forestMap;
        }
       
    }

    public void setTime(float time)
    {
        timeTextValue.text = time.ToString("0");
    }

    public void setMaxPlayer(float playerCount)
    {
        maxPlayerTextValue.text = playerCount.ToString("0");
    }

    public void HostLobby()
    {
        if (!hasAnyError())
        {
            return;
        }

        float timeValue = timeSlider.value;
        float maxPlayerValue = maxPlayerSlider.value;
        String roomName = roomNameField.text;
        String roomDesc = roomDescField.text;
        int map = mapDropdown.value;

        var instance = DinowarsNetworkManager.Instance;

        instance.GameTime = (int) Math.Round(timeValue);
        instance.maxConnections = (int) Math.Round(maxPlayerValue);
        instance.RoomName = roomName;
        instance.RoomDesc = roomDesc;
        instance.MapIndexValue = map;

        string hostName = Dns.GetHostName(); // Retrive the Name of HOST

        string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();

        instance.IpAddress = myIP;
       
        instance.StartHost();
        landingPanelUI.SetActive(false);
        lobbyPanelUI.SetActive(true);
    }

    private bool hasAnyError()
    {
        if (roomNameField.text.Equals("")){
            errorTextPanel.SetActive(true);
            errorText.text = "* Please enter room name";
            return false;
        }
        else if (roomDescField.text.Equals(""))
        {
            errorTextPanel.SetActive(true);
            errorText.text = "* Please enter room description";
            return false;
        }
        else
        {
            errorTextPanel.SetActive(false);
            errorText.text = "";
            return true;
        }
    }
}
