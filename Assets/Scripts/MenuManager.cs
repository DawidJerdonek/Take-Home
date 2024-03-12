using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject displayLeaderboard;

    public void viewLeaderboard()
    {
        displayLeaderboard.GetComponent<LeaderboardDisplay>().SetLeaderboardData(NetworkManager.brainCloudHighscoreLeaderboardID);
        displayLeaderboard.SetActive(true);
    }
    public void LoginScene()
    {
        SceneManager.LoadScene("Login");
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
