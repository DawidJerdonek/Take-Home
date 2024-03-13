using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrainCloud.LitJson;
using static NetworkManager;
using UnityEditor.Compilation;

public class NetworkManager : MonoBehaviour
{
    [Header("Leaderboard Constants")]
    public const string brainCloudHighscoreLeaderboardID = "Highscore";
    public const int brainCloudDefaultMinHighscoreIndex = 0;
    public const int brainCloudDefaultMaxHighscoreIndex = 9;

    [Header("User Stats Constants")]
    public const string brainCloudStatCookiesClicked = "CookiesClicked";
    public const string brainCloudStatMinutesElapsed = "MinutesElapsed";
    public const string brainCloudStatSecondsElapsed = "SecondsElapsed";

    [Header("Achievement Constants")]
    public const string brainCloudAchievementClick100 = "100Cookies";
    public const string brainCloudAchievementClick500 = "500Cookies";


    public static readonly Dictionary<string, string> brainCloudDescriptions = new Dictionary<string, string>
    { { brainCloudStatCookiesClicked,"Number of Cookies clicked by a user" }, 
        {brainCloudStatMinutesElapsed ,"Minutes the user clicked for"}, 
        { brainCloudStatSecondsElapsed,"Seconds user clicked for"} };

    public delegate void AuthenticationReqCompleted();
    public delegate void AuthenticationReqFailed();
    public delegate void LogoutCompleted();
    public delegate void LogoutFailed();
    public delegate void LeaderboardReqCompleted(Leaderboard leaderboard);
    public delegate void LeaderboardReqFailed();
    public delegate void PostScoreReqCompleted();
    public delegate void PostScoreReqFailed();
    public delegate void UserStatisticsReqCompleted(ref List<Statistics> statistics);
    public delegate void UserStatisticsReqFailed();
    public delegate void IncrementUserStatisticsCompleted(ref List<Statistics> statistics);
    public delegate void IncrementUserStatisticsFailed();
    public delegate void AchievementReqCompleted(ref List<Achievement> achievements);
    public delegate void AchievementReqFailed();


    public static NetworkManager sharedInstance;
    public bool isPlayerUniversallyAuthenticated;

    private BrainCloudWrapper m_brainCloud;
    private string m_username;

    private void Awake()
    {
        sharedInstance = this;
        DontDestroyOnLoad(gameObject);

        m_brainCloud = gameObject.AddComponent<BrainCloudWrapper>();
        m_brainCloud.Init();
        Debug.Log("Initialised");

    }

    void Update()
    {
        m_brainCloud.RunCallbacks();
    }


    public bool AuthenticatedPreviously()
    {
        return m_brainCloud.GetStoredProfileId() != "" && m_brainCloud.GetStoredProfileId() != "";
    }

    public bool AuthenticationStatus()
    {
        return m_brainCloud.Client.Authenticated;
    }

    public bool UsernameSaved()
    {
        return m_username != "";
    }

    public void SetUserName(string username)
    {
        m_username = username;
    }

    public string GetUsername()
    {
        return m_username;
    }

    public void HandleAuthenticationSuccess(string responseData, object cbObject, AuthenticationReqCompleted authenticationReqCompleted)
    {
        if (authenticationReqCompleted != null)
        {
            authenticationReqCompleted();
        }
    }

    public void RequestLeaderboard(string leaderboardId, LeaderboardReqCompleted leaderboardReqCompleted = null,
        LeaderboardReqFailed leaderboardReqFailed = null)
    {
        RequestLeaderboard(leaderboardId, brainCloudDefaultMinHighscoreIndex, brainCloudDefaultMaxHighscoreIndex, leaderboardReqCompleted, leaderboardReqFailed);
    }

    public void RequestLeaderboard(string leaderboardId, int startIndex, int endIndex, LeaderboardReqCompleted leaderboardReqCompleted = null,
        LeaderboardReqFailed leaderboardReqFailed = null)
    {
        if(AuthenticationStatus())
        {
            //Successful callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("Request Leaderboard SUCCEEDED" + responseData);

                JsonData jsonData = JsonMapper.ToObject(responseData);
                JsonData leaderboard = jsonData["data"]["leaderboard"];

                List<EnterLeaderboard> leaderboardListEntries = new List<EnterLeaderboard>();
                int rank = 0;
                string nickname;
                long ms = 0;
                float time = 0.0f;

                if(leaderboard.IsArray)
                {
                    for(int i = 0; i < leaderboard.Count; i++)
                    {
                        rank = int.Parse(leaderboard[i]["rank"].ToString());
                        nickname = leaderboard[i]["data"]["nickname"].ToString();
                        ms = long.Parse(leaderboard[i]["score"].ToString());
                        time = (float)ms / 1000.0f;

                        leaderboardListEntries.Add(new EnterLeaderboard(nickname, time, rank));
                    }
                }

                //
                //
                Leaderboard lb = new Leaderboard(leaderboardId, leaderboardListEntries); //Comes out as null???
                //
                //

                if (leaderboardReqCompleted != null)
                {
                    leaderboardReqCompleted(lb);
                }
            };

            //Failed callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log("Leaderboard Request FAILED: " + statusMessage);

                if (leaderboardReqFailed != null)
                {
                    leaderboardReqFailed();
                }
            };

            //brainCloud Request
            m_brainCloud.LeaderboardService.GetGlobalLeaderboardPage(leaderboardId, BrainCloud.BrainCloudSocialLeaderboard.SortOrder.HIGH_TO_LOW,
                startIndex, endIndex, successCallback, failureCallback);
        }
        else
        {
            Debug.Log("Leaderboard Request FAILED: no authentication");

            if(leaderboardReqFailed != null)
            {
                leaderboardReqFailed();
            }
        }
    }

    public void Logout(LogoutCompleted logoutCompleted = null, LogoutFailed logoutFailed = null)
    {
        if(AuthenticationStatus())
        {
            //Successful callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                isPlayerUniversallyAuthenticated = false;
                Debug.Log("Logout SUCCEEDED: " + responseData);
                m_brainCloud.ResetStoredAnonymousId();
                m_brainCloud.ResetStoredProfileId();

                if(logoutCompleted != null)
                {
                    logoutCompleted();
                }
            };
            //Failed callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log("Logout FAILED: " + statusMessage);
                m_brainCloud.ResetStoredAnonymousId();
                m_brainCloud.ResetStoredProfileId();

                if (logoutFailed != null)
                {
                    logoutFailed();
                }
            };

            m_brainCloud.PlayerStateService.Logout(successCallback, failureCallback);
        }
        else
        {
            Debug.Log("Logout FAILED: User was never authenticated");

            if (logoutFailed != null)
            {
                logoutFailed();
            }
        }
    }

    public void RequestUniversalAuthentication(string username, string password, AuthenticationReqCompleted authenticationReqCompleted = null,
        AuthenticationReqFailed authenticationReqFailed = null)
    {
        //Successful callback lambda
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            isPlayerUniversallyAuthenticated = true;
            Debug.Log("Universal Authentication SUCCEEDED: " + responseData);
            HandleAuthenticationSuccess(responseData, cbObject, authenticationReqCompleted);
        };

        //Failed callback lambda
        BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
        {
            isPlayerUniversallyAuthenticated = false;
            Debug.Log("Universal Authentication FAILED: " + statusMessage);
            if (authenticationReqFailed != null)
            {
                authenticationReqFailed();
            }
        };

        //brainCloud Universal Authentication
        m_brainCloud.AuthenticateUniversal(username, password, true, successCallback, failureCallback); //True to create auth if no profile exists
    }


    public void Reconnect(AuthenticationReqCompleted authenticationReqCompleted = null, AuthenticationReqFailed authenticationReqFailed = null)
    {
        //Successful callback lambda
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            isPlayerUniversallyAuthenticated = true;
            Debug.Log("Reconnect Authentication SUCCEEDED: " + responseData);
            HandleAuthenticationSuccess(responseData, cbObject, authenticationReqCompleted);
        };

        //Failed callback lambda
        BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
        {
            isPlayerUniversallyAuthenticated = false;
            Debug.Log("Reconnect Authentication FAILED: " + statusMessage);
            if (authenticationReqFailed != null)
            {
                authenticationReqFailed();
            }
        };

        //brainCloud Reconnect
        m_brainCloud.Reconnect(successCallback, failureCallback);
    }

    public void ReqAnonymousAuthentication(AuthenticationReqCompleted authenticationReqCompleted = null,
        AuthenticationReqFailed authenticationReqFailed = null)
    {
        //Successful callback lambda
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            isPlayerUniversallyAuthenticated = false;
            Debug.Log("AnonymouseAuthentication Request SUCCEEDED: " + responseData);
            HandleAuthenticationSuccess(responseData, cbObject, authenticationReqCompleted);
        };

        //Failed callback lambda
        BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
        {
            isPlayerUniversallyAuthenticated = false;
            Debug.Log("AnonymouseAuthentication Request FAILED: " + statusMessage);
            if (authenticationReqFailed != null)
            {
                authenticationReqFailed();
            }
        };

        //brainCloud Request
        m_brainCloud.AuthenticateAnonymous(successCallback, failureCallback);

    }

    public void PostScoreToLeaderboard(string leaderboardID, float time, PostScoreReqCompleted postScoreReqCompleted = null,
        PostScoreReqFailed postScoreReqFailed = null)
    {
        PostScoreToLeaderboard(leaderboardID, time, GetUsername(), postScoreReqCompleted, postScoreReqFailed);
    }

    public void PostScoreToLeaderboard(string leaderboardID, float time, string nickname, PostScoreReqCompleted postScoreReqCompleted = null,
        PostScoreReqFailed postScoreReqFailed = null)
    {
        if(AuthenticationStatus())
        {
            //Successful callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("Posting Score to Leaderboard SUCCEEDED: " + responseData);
                if (postScoreReqCompleted != null)
                {
                    postScoreReqCompleted();
                }
            };
            //Failure callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log("Posting Score to Leaderboard FAILED: " + statusMessage);
                if (postScoreReqFailed != null)
                {
                    postScoreReqFailed();
                }
            };

            //brainCloud Request
            long score = (long)(time * 1000.0f);
            string jsonOtherData = "{\"nickname\":\"" + nickname + "\"}";
            m_brainCloud.LeaderboardService.PostScoreToLeaderboard(leaderboardID, score, jsonOtherData, successCallback, failureCallback);

        }
        else
        {
            Debug.Log("Post Score to Leaderboard FAILED: user not authenticated");
            if(postScoreReqFailed != null)
            {
                postScoreReqFailed();
            }
        }
    }

    public void ReqUserStatistics(UserStatisticsReqCompleted userStatisticsReqCompleted = null, UserStatisticsReqFailed userStatisticsReqFailed = null)
    {
        if(AuthenticationStatus())
        {
            //Successful callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("User Statistics request SUCCEEDED: " + responseData);
                //Read Json
                JsonData data = JsonMapper.ToObject(responseData);
                JsonData statistics = data["data"]["statistics"];

                List<Statistics> statisticsList = new List<Statistics>();

                long value = 0;
                string description;

                foreach(string key in statistics.Keys)
                {
                    value = long.Parse(statistics[key].ToString());
                    description = brainCloudDescriptions[key];
                    statisticsList.Add(new Statistics(key, description, value));
                }

                if(userStatisticsReqCompleted != null)
                {
                    userStatisticsReqCompleted(ref statisticsList);
                }
            };

            //Failed callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log("User Statistics Request FAILED: " + statusMessage);

                if(userStatisticsReqFailed != null)
                {
                    userStatisticsReqFailed();
                }
            };

            //Make brainCloud request
            m_brainCloud.PlayerStatisticsService.ReadAllUserStats(successCallback, failureCallback);
        }
        else
        {
            Debug.Log("User Statistics Request FAILED: User not Authenticated");

            if(userStatisticsReqFailed != null)
            {
                userStatisticsReqFailed();
            }
        }

    }

    public void IncrementUserStatistics(Dictionary<string, object> data, IncrementUserStatisticsCompleted incrementUserStatisticsCompleted = null,
        IncrementUserStatisticsFailed incrementUserStatisticsFailed = null)
    {
        if(AuthenticationStatus())
        {
            //Successful Callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("User Statistics Increment SUCCEEDED: " + responseData);

                JsonData jsonData = JsonMapper.ToObject(responseData);
                JsonData statistics = jsonData["data"]["statistics"];
                List<Statistics> statisticsList = new List<Statistics>();

                long value = 0;
                string description;
                foreach (string key in statistics.Keys)
                {
                    value = long.Parse(statistics[key].ToString());
                    description = brainCloudDescriptions[key];
                    statisticsList.Add(new Statistics(key, description, value));
                }

                if (incrementUserStatisticsCompleted != null)
                {
                    incrementUserStatisticsCompleted(ref statisticsList);
                }
            };

            //Failed Callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log("User Statistics Increment FAILED: " + statusMessage);

                if (incrementUserStatisticsFailed != null)
                {
                    incrementUserStatisticsFailed();
                }
            };

            //brainCloud Request
            m_brainCloud.PlayerStatisticsService.IncrementUserStats(data,successCallback,failureCallback);
        }
        else
        {
            Debug.Log("User Statistics Increment FAILED: User not Authenticated");
            if(incrementUserStatisticsFailed != null)
            {
                incrementUserStatisticsFailed();
            }
        }
    }

    public void RequestAchievements(AchievementReqCompleted achievementReqCompleted = null, AchievementReqFailed achievementReqFailed = null)
    {
        if(AuthenticationStatus())
        {
            //Successful Callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("Achievement Request SUCCEEDED: " + responseData);

                JsonData jsonData = JsonMapper.ToObject(responseData);
                JsonData achievements = jsonData["data"]["achievements"];
                List<Achievement> achievementsList = new List<Achievement>();

                string id;
                string title;
                string description;
                string status;

                if(achievements.IsArray)
                {
                    for(int i = 0; i< achievements.Count; i++)
                    {
                        id = achievements[i]["id"].ToString();
                        title = achievements[i]["title"].ToString();
                        description = achievements[i]["description"].ToString();
                        status = achievements[i]["status"].ToString();

                        achievementsList.Add(new Achievement(id, title, description, status));
                    }
                }

                if (achievementReqCompleted != null)
                {
                    achievementReqCompleted(ref achievementsList);
                }
            };

            //Failed Callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log("Achievement Request FAILED: " + statusMessage);

                if (achievementReqFailed != null)
                {
                    achievementReqFailed();
                }
            };

            //brainCloud Request
            m_brainCloud.GamificationService.ReadAchievements(true, successCallback, failureCallback);
        }
        else
        {
            Debug.Log("Achievement Request FAILED: User not Authenticated");

            if (achievementReqFailed != null)
            {
                achievementReqFailed();
            }
        }
    }
}
