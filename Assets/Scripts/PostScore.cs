using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostScore : MonoBehaviour
{
    [SerializeField] private TMP_InputField leaderboardUsernameInputField;
    private NetworkManager.PostScoreReqCompleted m_postScoreReqCompleted;
    private NetworkManager.PostScoreReqFailed m_postScoreReqFailed;

    private float m_time;

    public void RequestPost(float time, NetworkManager.PostScoreReqCompleted postScoreReqCompleted = null,
        NetworkManager.PostScoreReqFailed postScoreReqFailed = null)
    {
        Set(time,postScoreReqCompleted, postScoreReqFailed);
    }

    public void Set(float time, NetworkManager.PostScoreReqCompleted postScoreReqCompleted, NetworkManager.PostScoreReqFailed postScoreReqFailed)
    {
        m_time = time;

        m_postScoreReqCompleted = postScoreReqCompleted;
        m_postScoreReqFailed = postScoreReqFailed;
    }

    public void SubmitScore()
    {
        Statistics cookieStat = StatisticsManager.instance.GetStatisticByName("CookiesClicked");
        
        NetworkManager.sharedInstance.PostScoreToLeaderboard(NetworkManager.brainCloudHighscoreLeaderboardID, cookieStat.Value, leaderboardUsernameInputField.text, m_postScoreReqCompleted, m_postScoreReqFailed);
    }
}
