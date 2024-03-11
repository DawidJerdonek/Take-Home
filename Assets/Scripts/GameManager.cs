using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

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
        NetworkManager.sharedInstance.ReqAnonymousAuthentication(OnAuthenticationReqCompleted);
    }

    public void OnAuthenticationReqCompleted()
    {
        
    }
}
