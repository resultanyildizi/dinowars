using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TeamScoreCard : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text killText;
    [SerializeField] Text deathText;


    public void UpdateCard(string name, int kill, int death)
    {
        nameText.text = name;
        killText.text = kill.ToString();
        deathText.text = death.ToString();
    }

    public void ResetCard()
    {
        nameText.text = "Player";
        killText.text = "-";
        deathText.text = "-";
    }
}
