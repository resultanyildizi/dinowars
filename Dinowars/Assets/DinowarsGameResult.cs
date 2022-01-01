using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class DinowarsGameResult : NetworkBehaviour
{
    public enum Result
    {
        WIN, LOSE, DRAW
    }

    [SyncVar]
    private int teamAScore;

    [SyncVar]
    private int teamBScore;

    [SyncVar]
    private Result result;


    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject confetties;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject drawText;
    [SerializeField] private GameObject uxgylSad;
    [SerializeField] private GameObject rextSad;

    [SerializeField] private Text teamAScoreText;
    [SerializeField] private Text teamBScoreText;

    public void SetPlayer(Result result, int teamAScore, int teamBScore)
    {
        this.result = result;
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

        winText.SetActive(result == Result.WIN);
        confetties.SetActive(result == Result.WIN);

        loseText.SetActive(result == Result.LOSE);
        uxgylSad.SetActive(result == Result.LOSE);
        rextSad.SetActive(result == Result.LOSE);

        drawText.SetActive(result == Result.DRAW);

        teamAScoreText.text = teamAScore.ToString();
        teamBScoreText.text = teamBScore.ToString();
    }



}
