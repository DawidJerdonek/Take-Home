using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    public LoginHandler loginHandler;
    private float m_elapsedTime;

    private void Awake()
    {

    }

    void Start()
    {
        if (gameManagerInstance != null && gameManagerInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }

        HandleAuthentication();
    }

    void Update()
    {
        m_elapsedTime += Time.deltaTime;
    }

    public void HandleAuthentication() 
    {
        if(NetworkManager.sharedInstance.AuthenticatedPreviously())
        {
            NetworkManager.sharedInstance.Reconnect(OnAuthenticationReqCompleted); //All forms of Auth
        }
        else
        {
            //NetworkManager.sharedInstance.ReqAnonymousAuthentication(OnAuthenticationReqCompleted);
            loginHandler.RequestLogin();
        }
    }

    private void OnLeaderboardRequestCompleted(Leaderboard leaderboard)
    {
        LeaderboardsManager.instance.AddLeaderboard(leaderboard);
    }

    public void OnAuthenticationReqCompleted()
    {
        NetworkManager.sharedInstance.RequestLeaderboard(NetworkManager.brainCloudHighscoreLeaderboardID, OnLeaderboardRequestCompleted);
        NetworkManager.sharedInstance.ReqUserStatistics(OnUserStatisticsReqCompleted);
    }

    private void OnPostScoreReqCompleted()
    {
        LeaderboardsManager.instance.SetUserTime(m_elapsedTime);
        NetworkManager.sharedInstance.RequestLeaderboard(NetworkManager.brainCloudHighscoreLeaderboardID, OnLeaderboardRequestCompleted);
    }

    private void OnUserStatisticsReqCompleted(ref List<Statistics> statistics)
    {
        StatisticsManager.instance.SetStatistics(ref statistics);
    }

    public void OnUserStatisticsIncrementCompleted(ref List<Statistics> statistics)
    {
        StatisticsManager.instance.SetStatistics(ref statistics);
    }
}
