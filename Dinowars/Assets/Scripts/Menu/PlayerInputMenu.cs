using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputMenu : MonoBehaviour
{
    [Header("Menu UI")]
    [SerializeField] private InputField usernameInputField;
    [SerializeField] private Button continueButton;

    public const string PlayerPrefsNameKey = "PlayerUsername";

    private void Start() => SetUpInputField();

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        usernameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        PlayerPrefs.SetString(PlayerPrefsNameKey, usernameInputField.text);
    }
}
