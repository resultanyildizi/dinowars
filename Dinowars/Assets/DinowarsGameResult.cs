using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class DinowarsGameResult : NetworkBehaviour
{
    [SyncVar]
    private int teamAScore;

    [SyncVar]
    private int teamBScore;

    [SyncVar]
    private bool isWinner;


    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject confetties;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject uxgylSad;
    [SerializeField] private GameObject rextSad;

    [SerializeField] private Text teamAScoreText;
    [SerializeField] private Text teamBScoreText;

    public void SetPlayer(bool isWinner, int teamAScore, int teamBScore)
    {
        this.isWinner = isWinner;
        this.teamAScore = teamAScore;
        this.teamBScore = teamBScore;
    }

    public override void OnStartAuthority()
    {
        gameObject.SetActive(hasAuthority);
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);

        if (!hasAuthority)
        {
            NetworkServer.Destroy(gameObject);
            GameObject.Destroy(gameObject);
        }
        gameObject.SetActive(hasAuthority);

        winText.SetActive(isWinner);
        confetties.SetActive(isWinner);

        loseText.SetActive(!isWinner);
        uxgylSad.SetActive(!isWinner);
        rextSad.SetActive(!isWinner);

        teamAScoreText.text = teamAScore.ToString();
        teamBScoreText.text = teamBScore.ToString();
    }



}
