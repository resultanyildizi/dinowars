using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class DinowarsScoreBoard : MonoBehaviour
{
    [SerializeField] private Text teamAScore;
    [SerializeField] private Text teamBScore;
    [SerializeField] private Text timer;
    [SerializeField] private TeamScoreCard[] teamAScoreCards;
    [SerializeField] private TeamScoreCard[] teamBScoreCards;

    private int teamATotalScore = 0;
    private int teamBTotalScore = 0;

    private void OnEnable()
    {
        UpdateScoreCards();
        UpdateTeamScores();
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        var gp =  Utils.GetGameObjectFromConnection(NetworkClient.localPlayer.connectionToClient);
        gp.TimerChanged += ChangeTimerText;
    }

    private void ChangeTimerText(int time)
    {
        timer.text = time.ToString();
    }


    private void UpdateTeamScores()
    {
        int newTotalAScore = 0;
        for (int i = 0; i < DinowarsNetworkManager.Instance.TeamAGamePlayers.Count; i++)
        {
            var gamePlayer = DinowarsNetworkManager.Instance.TeamAGamePlayers[i];
            newTotalAScore += gamePlayer.Killed;
        }

        int newTotalBScore = 0;
        for (int i = 0; i < DinowarsNetworkManager.Instance.TeamBGamePlayers.Count; i++)
        {
            var gamePlayer = DinowarsNetworkManager.Instance.TeamBGamePlayers[i];
            newTotalBScore += gamePlayer.Killed;
        }

        teamATotalScore = newTotalAScore;
        teamBTotalScore = newTotalBScore;

        teamAScore.text = teamATotalScore.ToString();
        teamBScore.text = teamBTotalScore.ToString();
    }

    private void UpdateScoreCards()
    {
        ResetCards();

        for (int i = 0; i < DinowarsNetworkManager.Instance.TeamAGamePlayers.Count; i++)
        {
            var gamePlayer = DinowarsNetworkManager.Instance.TeamAGamePlayers[i];
            teamAScoreCards[i].UpdateCard(gamePlayer.DisplayName, gamePlayer.Killed, gamePlayer.Death);
        }

        for (int i = 0; i < DinowarsNetworkManager.Instance.TeamBGamePlayers.Count; i++)
        {
            var gamePlayer = DinowarsNetworkManager.Instance.TeamBGamePlayers[i];
            teamBScoreCards[i].UpdateCard(gamePlayer.DisplayName, gamePlayer.Killed, gamePlayer.Death);
        }

    }

    public void ResetCards()
    {
        foreach (var card in teamAScoreCards) card.ResetCard();
        foreach (var card in teamBScoreCards) card.ResetCard();
    }

    // void Start()
    // {

    // }


    // void Update()
    // {

    // }
}
