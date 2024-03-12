using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrainCloud.LitJson;

public class NetworkManager : MonoBehaviour
{
    public const string brainCloudHighscoreLeaderboardID = "Highscore";
    public const int brainCloudDefaultMinHighscoreIndex = 0;
    public const int brainCloudDefaultMaxHighscoreIndex = 9;

    public delegate void AuthenticationReqCompleted();
    public delegate void AuthenticationReqFailed();
    public delegate void LogoutCompleted();
    public delegate void LogoutFailed();
    public delegate void LeaderboardReqCompleted(Leaderboard leaderboard);
    public delegate void LeaderboardReqFailed();
    public delegate void PostScoreReqCompleted();
    public delegate void PostScoreReqFailed();

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
}
