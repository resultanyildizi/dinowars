using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText;

    public Text PlayerNameText { get => playerNameText; set => playerNameText = value; }
}
