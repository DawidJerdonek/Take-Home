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

    void Start()
    {
        
    }

    void Update()
    {
        brainCloud.RunCallbacks();
    }

    public bool AuthenticationStatus()
    {
        return brainCloud.Client.Authenticated;
    }

    public void ReqAnonymousAuthentication(AuthenticationReqCompleted authenticationReqCompleted = null,
        AuthenticationReqFailed authenticationReqFailed = null)
    {
        //Successfull callback lambda
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            Debug.Log("AnonymouseAuthentication Request SUCCEEDED: " + responseData);
            if (authenticationReqCompleted != null)
            {
                authenticationReqCompleted();
            }
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
