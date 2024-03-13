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

    public long cookieClicked = 0;

    private float timer = 0;
    private float hours = 0;
    private float minutes = 0;
    private float seconds = 0;

    private bool doubleClicks = false;
    private bool quadraClicks = false;
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            cookie = cookie.GetComponent<Button>();
            cookie.onClick.AddListener(CookieClicked);
            SetCookieStat();
        }
    }

    void Update()
    {
        //Cookie clicked!
        timesClickedText.text = "Cookie Clicked: " + cookieClicked + " Times!";

        //Time spent clicking
        timer += Time.deltaTime;
        minutes = Mathf.FloorToInt(timer / 60);
        seconds = Mathf.FloorToInt(timer % 60);
        hours = Mathf.FloorToInt(minutes % 60);
        timeSpentText.text = "You have been clicking for:\n" + hours + " Hours, " + minutes + " Minutes, and " + seconds + " Seconds!";

        CheckCookieAchievements();

        //Update Statistics
        //Statistics secondsStat = StatisticsManager.instance.GetStatisticByName(NetworkManager.brainCloudStatSecondsElapsed);
        //SecondsStat.increment();


    }

    void CheckCookieAchievements()
    {
        Achievement click100 = AchievementManager.instance.GetAchievementById(NetworkManager.brainCloudAchievementClick100);
        if(click100 != null && !click100.Awarded && cookieClicked >= 100)
        {
            click100.AwardAchievement();
            doubleClicks = true;
        }

        Achievement click500 = AchievementManager.instance.GetAchievementById(NetworkManager.brainCloudAchievementClick500);
        if (click500 != null && !click500.Awarded && cookieClicked >= 500)
        {
            click500.AwardAchievement();
            quadraClicks = true;
        }
    }

    void CookieClicked()
    {
        

        //Update Statistics
        Statistics numberClicked = StatisticsManager.instance.GetStatisticByName(NetworkManager.brainCloudStatCookiesClicked);

        if (quadraClicks)
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
    }

    public void SaveStats()
    {

        //Send to brainCloud 
        Dictionary<string, object> dictionary = StatisticsManager.instance.GetIncrementsDictionary();
        if (dictionary != null)
        {
            NetworkManager.sharedInstance.IncrementUserStatistics(dictionary, GameManager.gameManagerInstance.OnUserStatisticsIncrementCompleted);
        }
    }
}
