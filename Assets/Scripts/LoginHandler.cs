using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] List<GameObject> LoginPanel = new List<GameObject>();
    [SerializeField] private List<GameObject> nonLoginPanel = new List<GameObject>();

    private NetworkManager.AuthenticationReqCompleted m_authenticationReqCompleted;
    private NetworkManager.AuthenticationReqFailed m_authenticationReqFailed;

    private void Start()
    {
        for (int i = 0; i < LoginPanel.Count; i++)
        {
            LoginPanel[i].SetActive(false);
        }
        for (int i = 0; i < nonLoginPanel.Count; i++)
        {
            nonLoginPanel[i].SetActive(true);
        }
    }

    public void Set(NetworkManager.AuthenticationReqCompleted authenticationReqCompleted, NetworkManager.AuthenticationReqFailed authenticationReqFailed)
    {
        m_authenticationReqCompleted = authenticationReqCompleted;
        m_authenticationReqFailed = authenticationReqFailed;
    }

    public void RequestLogin(NetworkManager.AuthenticationReqCompleted authenticationReqCompleted = null, NetworkManager.AuthenticationReqFailed authenticationReqFailed = null)
    {
        Set(authenticationReqCompleted,authenticationReqFailed);
        for(int i = 0; i < LoginPanel.Count; i++)
        {
            LoginPanel[i].SetActive(true);
        }
        for (int i = 0; i < nonLoginPanel.Count; i++)
        {
            nonLoginPanel[i].SetActive(false);
        }
    }

    public void ConfirmLogin()
    {
        NetworkManager.sharedInstance.RequestUniversalAuthentication(usernameInput.text,passwordInput.text, 
            m_authenticationReqCompleted,m_authenticationReqFailed);

        for (int i = 0; i < LoginPanel.Count; i++)
        {
            LoginPanel[i].SetActive(false);
        }
        for (int i = 0; i < nonLoginPanel.Count; i++)
        {
            nonLoginPanel[i].SetActive(true);
        }

    }

    public void ConfirmLogout()
    {
        NetworkManager.sharedInstance.Logout();
    }
}
