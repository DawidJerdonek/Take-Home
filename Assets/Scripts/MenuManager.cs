using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [SerializeField] private List<GameObject> defaultPanel = new List<GameObject>();
    [SerializeField] private Button loginButton;
    [SerializeField] private TextMeshProUGUI loggedInAsText;

    [Header("Leaderboard")]
    [SerializeField] private GameObject displayLeaderboard;

    [Header("Statistics")]
    [SerializeField] private GameObject displayStatistics;
    [SerializeField] private TextMeshProUGUI cookiesClickedText;
    [SerializeField] private TextMeshProUGUI timeElapsedText;

    [Header("Chat")]
    public GameObject displayChat;
    [SerializeField] private GameObject showChatButton;

    [Header("Achievements")]
    [SerializeField] private GameObject displayAchievements;
    [SerializeField] private TextMeshProUGUI cookies100Text;
    [SerializeField] private TextMeshProUGUI cookies500Text;
    [SerializeField] private TextMeshProUGUI cookies10KText;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            displayChat.GetComponent<LeaderboardDisplay>().SetLeaderboardData(NetworkManager.brainCloudChatLeaderboardID);
        }
        if (SceneManager.GetActiveScene().name == "LogIn")
        {
            if(loggedInAsText != null)
            {
                loggedInAsText.text ="Logged in As:\n" + NetworkManager.instance.GetUsername();
            }
            
        }
    }

    public void ViewChat()
    {
        showChatButton.SetActive(false);
        displayChat.GetComponent<LeaderboardDisplay>().SetLeaderboardData(NetworkManager.brainCloudChatLeaderboardID);
        displayChat.SetActive(true);

    }

    public void StopViewingChat()
    {
        showChatButton.SetActive(true);
        displayChat.SetActive(false);
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

    public void ViewAchievements()
    {
        for (int i = 0; i < defaultPanel.Count; i++)
        {
            defaultPanel[i].SetActive(false);
        }

        cookies100Text.text = "Collect 100 Cookies";
        cookies500Text.text = "Collect 500 Cookies";
        cookies10KText.text = "Collect 10,000 Cookies";

        Achievement click100 = AchievementManager.instance.GetAchievementById(NetworkManager.brainCloudAchievementClick100);
        Achievement click500 = AchievementManager.instance.GetAchievementById(NetworkManager.brainCloudAchievementClick500);
        Achievement click10000 = AchievementManager.instance.GetAchievementById(NetworkManager.brainCloudAchievementClick10000);

        Debug.Log(click100.Awarded);

        if (click100.Awarded)
        {
            cookies100Text.color = Color.green;
        }
        else { cookies100Text.color = Color.red; }

        if (click500.Awarded)
        {
            cookies500Text.color = Color.green;
        }
        else { cookies500Text.color = Color.red; }

        if (click10000.Awarded)
        {
            cookies10KText.color = Color.green;
        }
        else { cookies10KText.color = Color.red; }

        displayAchievements.SetActive(true);
    }

    public void StopViewingAchievements()
    {
        for (int i = 0; i < defaultPanel.Count; i++)
        {
            defaultPanel[i].SetActive(true);
        }
        displayAchievements.SetActive(false);
    }

    public void ViewStatistics()
    {
        for (int i = 0; i < defaultPanel.Count; i++)
        {
            defaultPanel[i].SetActive(false);
        }

        Statistics cookieStat = StatisticsManager.instance.GetStatisticByName("CookiesClicked");
        cookiesClickedText.text = "You collected: " + cookieStat.Value + " Cookies!";

        Statistics timeStat = StatisticsManager.instance.GetStatisticByName("TimeElapsed");

        int minutes = Mathf.FloorToInt(timeStat.Value / 60);
        int seconds = Mathf.FloorToInt(timeStat.Value % 60);
        int hours = Mathf.FloorToInt(minutes / 60);

        timeElapsedText.text = "You spent: " + hours + " Hours, " + minutes + " Minutes, and " + seconds + " Seconds, Clicking Cookies";

        displayStatistics.SetActive(true);
    }

    public void StopViewingStats()
    {
        for (int i = 0; i < defaultPanel.Count; i++)
        {
            defaultPanel[i].SetActive(true);
        }
        displayStatistics.SetActive(false);
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
