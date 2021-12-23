using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Text roundTextValue = null;
    [SerializeField] Slider roundSlider = null;
    [SerializeField] Text maxPlayerTextValue = null;
    [SerializeField] Slider maxPlayerSlider = null;
    [SerializeField] InputField roomNameField = null;
    [SerializeField] InputField roomDescField = null;
    [SerializeField] Dropdown modeDropdown = null;
    [SerializeField] Dropdown mapDropdown = null;
    [SerializeField] Sprite forestMap = null;
    [SerializeField] Sprite caveMap = null;


    private void Update()
    {
        if(mapDropdown != null)
        {
            if (mapDropdown.value == 0)
            {
                this.settingPanelUI.GetComponent<Image>().sprite = caveMap;
            }
            else if (mapDropdown.value == 1)
            {
                this.settingPanelUI.GetComponent<Image>().sprite = forestMap;
            }
        }
       
    }

    public void setTime(float time)
    {
        timeTextValue.text = time.ToString("0");
    }

    public void setRound(float round)
    {
        roundTextValue.text = round.ToString("0");
    }

    public void setMaxPlayer(float playerCount)
    {
        maxPlayerTextValue.text = playerCount.ToString("0");
    }

    public void HostLobby()
    {
        float timeValue = timeSlider.value;
        float roundValue = roundSlider.value;
        float maxPlayerValue = maxPlayerSlider.value;
        String roomName = roomNameField.text;
        String roomDesc = roomDescField.text;
        int mode = modeDropdown.value;
        int map = mapDropdown.value;

        var instance = DinowarsNetworkManager.Instance;

        instance.timeValue = (int) Math.Round(timeValue);
        instance.roundValue = (int) Math.Round(roundValue);
        instance.maxConnections = (int) Math.Round(maxPlayerValue);
        instance.roomName = roomName;
        instance.roomDesc = roomDesc;
        instance.modeIndexValue = mode;
        instance.mapIndexValue = map;

        instance.StartHost();
        landingPanelUI.SetActive(false);
        lobbyPanelUI.SetActive(true);
    }
}
