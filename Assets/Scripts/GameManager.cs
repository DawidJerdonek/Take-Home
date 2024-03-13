using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    public LoginHandler loginHandler;
    private float m_elapsedTime;

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
        if(NetworkManager.instance.AuthenticatedPreviously())
        {
            NetworkManager.instance.Reconnect(OnAuthenticationReqCompleted); //All forms of Auth
        }
        else
        {
            //NetworkManager.sharedInstance.ReqAnonymousAuthentication(OnAuthenticationReqCompleted);
            loginHandler.RequestLogin();
        }
    }



    public void OnAuthenticationReqCompleted()
    {
        NetworkManager.instance.RequestLeaderboard(NetworkManager.brainCloudChatLeaderboardID, OnLeaderboardRequestCompleted);
        NetworkManager.instance.RequestLeaderboard(NetworkManager.brainCloudHighscoreLeaderboardID, OnLeaderboardRequestCompleted);
        NetworkManager.instance.ReqUserStatistics(OnUserStatisticsReqCompleted);
        NetworkManager.instance.RequestAchievements(OnAchievenemtReqCompleted);
    }

    public void OnLeaderboardRequestCompleted(Leaderboard leaderboard)
    {
        LeaderboardsManager.instance.AddLeaderboard(leaderboard);
    }

    private void OnPostScoreReqCompleted()
    {
        LeaderboardsManager.instance.SetUserTime(m_elapsedTime);
        NetworkManager.instance.RequestLeaderboard(NetworkManager.brainCloudHighscoreLeaderboardID, OnLeaderboardRequestCompleted);
    }

    private void OnUserStatisticsReqCompleted(ref List<Statistics> statistics)
    {
        StatisticsManager.instance.SetStatistics(ref statistics);
    }

    public void OnUserStatisticsIncrementCompleted(ref List<Statistics> statistics)
    {
        StatisticsManager.instance.SetStatistics(ref statistics);
    }

    private void OnAchievenemtReqCompleted(ref List<Achievement> achievements)
    {
        AchievementManager.instance.SetAchievements(ref achievements);
    }
}
