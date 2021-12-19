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
    public static string DisplayName;

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
        DisplayName = usernameInputField.text;
        Debug.Log(DisplayName);
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
       
    }
}
