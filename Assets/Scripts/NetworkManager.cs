using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public delegate void AuthenticationReqCompleted();
    public delegate void AuthenticationReqFailed();
    public delegate void LogoutCompleted();
    public delegate void LogoutFailed();

    public static NetworkManager sharedInstance;

    private BrainCloudWrapper brainCloud;

    private void Awake()
    {
        sharedInstance = this;
        DontDestroyOnLoad(gameObject);

        brainCloud = gameObject.AddComponent<BrainCloudWrapper>();
        brainCloud.Init();
        Debug.Log("Initialised");

    }

    void Update()
    {
        brainCloud.RunCallbacks();
    }

    public bool AuthenticatedPreviously()
    {
        return brainCloud.GetStoredProfileId() != "" && brainCloud.GetStoredProfileId() != "";
    }

    public bool AuthenticationStatus()
    {
        return brainCloud.Client.Authenticated;
    }

    public void HandleAuthenticationSuccess(string responseData, object cbObject, AuthenticationReqCompleted authenticationReqCompleted)
    {
        if (authenticationReqCompleted != null)
        {
            authenticationReqCompleted();
        }
    }

    public void Logout(LogoutCompleted logoutCompleted = null, LogoutFailed logoutFailed = null)
    {
        if(AuthenticationStatus())
        {
            //Successfull callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("Logout SUCCEEDED: " + responseData);
                brainCloud.ResetStoredAnonymousId();
                brainCloud.ResetStoredProfileId();

                if(logoutCompleted != null)
                {
                    logoutCompleted();
                }
            };
            //Failed callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log("Logout FAILED: " + statusMessage);
                brainCloud.ResetStoredAnonymousId();
                brainCloud.ResetStoredProfileId();

                if (logoutFailed != null)
                {
                    logoutFailed();
                }
            };

            brainCloud.PlayerStateService.Logout(successCallback, failureCallback);
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
            Debug.Log("Universal Authentication SUCCEEDED: " + responseData);
            HandleAuthenticationSuccess(responseData, cbObject, authenticationReqCompleted);
        };

        //Failed callback lambda
        BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
        {
            Debug.Log("Universal Authentication FAILED: " + statusMessage);
            if (authenticationReqFailed != null)
            {
                authenticationReqFailed();
            }
        };

        //brainCloud Universal Authentication
        brainCloud.AuthenticateUniversal(username, password, true, successCallback, failureCallback); //True to create auth if no profile exists
    }


    public void Reconnect(AuthenticationReqCompleted authenticationReqCompleted = null, AuthenticationReqFailed authenticationReqFailed = null)
    {
        //Successful callback lambda
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            Debug.Log("Reconnect Authentication SUCCEEDED: " + responseData);
            HandleAuthenticationSuccess(responseData, cbObject, authenticationReqCompleted);
        };

        //Failed callback lambda
        BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
        {
            Debug.Log("Reconnect Authentication FAILED: " + statusMessage);
            if (authenticationReqFailed != null)
            {
                authenticationReqFailed();
            }
        };

        //brainCloud Reconnect
        brainCloud.Reconnect(successCallback, failureCallback);
    }

    public void ReqAnonymousAuthentication(AuthenticationReqCompleted authenticationReqCompleted = null,
        AuthenticationReqFailed authenticationReqFailed = null)
    {
        //Successful callback lambda
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            Debug.Log("AnonymouseAuthentication Request SUCCEEDED: " + responseData);
            HandleAuthenticationSuccess(responseData, cbObject, authenticationReqCompleted);
        };

        //Failed callback lambda
        BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
        {
            Debug.Log("AnonymouseAuthentication Request FAILED: " + statusMessage);
            if (authenticationReqFailed != null)
            {
                authenticationReqFailed();
            }
        };

        //brainCloud Request
        brainCloud.AuthenticateAnonymous(successCallback, failureCallback);

    }
}
