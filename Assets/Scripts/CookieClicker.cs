using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CookieClicker : MonoBehaviour
{
    //public GameObject Cookie;
    [SerializeField] private Button cookie;
    [SerializeField] private Transform canvas;
    [SerializeField] private TextMeshProUGUI timesClickedText;
    [SerializeField] private TextMeshProUGUI timeSpentText;
    [SerializeField] private TextMeshProUGUI achieve100Text;
    [SerializeField] private TextMeshProUGUI achieve500Text;
    [SerializeField] private TextMeshProUGUI achieve10000Text;

    public long cookieClicked = 0;


    private float timerSinceLastSave = 0;
    private float timer = 0;
    private float hours = 0;
    private float minutes = 0;
    private float seconds = 0;

    private bool doubleClicks = false;
    private bool quadraClicks = false;
    private bool clicksX20 = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            cookie = cookie.GetComponent<Button>();
            cookie.onClick.AddListener(CookieClicked);
            SetCookieStat();
        }
        NetworkManager.sharedInstance.RequestLeaderboard(NetworkManager.brainCloudChatLeaderboardID,GameManager.gameManagerInstance.OnLeaderboardRequestCompleted);
    }

    void Update()
    {
        //Cookie clicked!
        timesClickedText.text = "Cookie Clicked: " + cookieClicked + " Times!";

        //Time spent clicking
        timerSinceLastSave += Time.deltaTime;
        timer += Time.deltaTime;
        minutes = Mathf.FloorToInt(timer / 60);
        seconds = Mathf.FloorToInt(timer % 60);
        hours = Mathf.FloorToInt(minutes / 60);
        timeSpentText.text = "You have been clicking for:\n" + hours + " Hours, " + minutes + " Minutes, and " + seconds + " Seconds!";

        CheckCookieAchievements();

        //Update Statistics
        //Statistics secondsStat = StatisticsManager.instance.GetStatisticByName(NetworkManager.brainCloudStatSecondsElapsed);
        //SecondsStat.increment();


    }

    void CheckCookieAchievements()
    {
        Color green = new Color32(127, 255, 32, 255);
        
        Achievement click100 = AchievementManager.instance.GetAchievementById(NetworkManager.brainCloudAchievementClick100);
        if(click100 != null && !click100.Awarded && cookieClicked >= 100)
        {
            click100.AwardAchievement();
            doubleClicks = true;
            achieve100Text.color = green;
            achieve100Text.text += "    x2 Clicks!";
            Debug.Log(click100.Awarded);
        }

        Achievement click500 = AchievementManager.instance.GetAchievementById(NetworkManager.brainCloudAchievementClick500);
        if (click500 != null && !click500.Awarded && cookieClicked >= 500)
        {
            click500.AwardAchievement();
            quadraClicks = true;
            achieve500Text.color = green;
            achieve500Text.text += "    x2 Clicks!";
        }

        Achievement click10000 = AchievementManager.instance.GetAchievementById(NetworkManager.brainCloudAchievementClick10000);
        if (click10000 != null && !click10000.Awarded && cookieClicked >= 10000)
        {
            click10000.AwardAchievement();
            clicksX20 = true;
            achieve10000Text.color = green;
            achieve10000Text.text += "    x5 Clicks!";
        }
    }


    void CookieClicked()
    {
        //Update Statistics
        Statistics numberClicked = StatisticsManager.instance.GetStatisticByName(NetworkManager.brainCloudStatCookiesClicked);

        if(clicksX20)
        {
            cookieClicked += 20;
            numberClicked.IncrementByValue(20);
        }
        else if (quadraClicks)
        {
            cookieClicked += 4;
            numberClicked.IncrementByValue(4);
        }
        else if (doubleClicks)
        {
            cookieClicked+= 2;
            numberClicked.IncrementByValue(2);
        }
        else
        {
            cookieClicked++;
            numberClicked.IncrementValue();
        }
    }

    void SetCookieStat()
    {
        Statistics cookieStat = StatisticsManager.instance.GetStatisticByName("CookiesClicked");
        cookieClicked = cookieStat.Value;

        Statistics timeStat = StatisticsManager.instance.GetStatisticByName("TimeElapsed");
        timer = timeStat.Value;
    }

    public void SaveStats()
    {

        Statistics timeElapsed = StatisticsManager.instance.GetStatisticByName(NetworkManager.brainCloudStatTimeElapsed);
        int sendTime = (int)timerSinceLastSave;
        timeElapsed.IncrementByValue(sendTime);

        //Send to brainCloud 
        Dictionary<string, object> dictionary = StatisticsManager.instance.GetIncrementsDictionary();
        if (dictionary != null)
        {
            NetworkManager.sharedInstance.IncrementUserStatistics(dictionary, GameManager.gameManagerInstance.OnUserStatisticsIncrementCompleted);
        }

        //Reset Timer Used to Increment
        timerSinceLastSave = 0;
    }

    public void SaveAndExit()
    {
        SaveStats();
        MenuManager.instance.MainMenuScene();
    }
}
