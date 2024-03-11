using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public delegate void AuthenticationReqCompleted();
    public delegate void AuthenticationReqFailed();

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

    public void Reconnect(AuthenticationReqCompleted authenticationReqCompleted = null, AuthenticationReqFailed authenticationReqFailed = null)
    {
        //Successfull callback lambda
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
        //Successfull callback lambda
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
