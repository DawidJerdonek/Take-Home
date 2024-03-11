using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    public LoginHandler loginHandler;

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
            loginHandler.RequestLogin(OnAuthenticationReqCompleted);
        }
    }

    public void OnAuthenticationReqCompleted()
    {
        
    }
}
