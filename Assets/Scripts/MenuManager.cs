using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject displayLeaderboard;
    [SerializeField] private List<GameObject> defaultPanel = new List<GameObject>();

    [SerializeField] private Button loginButton;

    public void Update()
    {
        //if (SceneManager.GetActiveScene().name == "LogIn")
        //{
        //    if (NetworkManager.sharedInstance.isPlayerUniversallyAuthenticated)
        //    {
        //        loginButton.gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        loginButton.gameObject.SetActive(true);
        //    }
        //}

    }
    public void ViewLeaderboard()
    {
        for (int i = 0; i < defaultPanel.Count; i++)
        {
            defaultPanel[i].SetActive(false);
        }
        displayLeaderboard.GetComponent<LeaderboardDisplay>().SetLeaderboardData(NetworkManager.brainCloudHighscoreLeaderboardID);
        displayLeaderboard.SetActive(true);

    }

    public void StopViewingLeaderboard()
    {
        for (int i = 0; i < defaultPanel.Count; i++)
        {
            defaultPanel[i].SetActive(true);
        }
        displayLeaderboard.SetActive(false);
    }

    public void GameplayScene()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void LoginScene()
    {
        SceneManager.LoadScene("LogIn");
    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
