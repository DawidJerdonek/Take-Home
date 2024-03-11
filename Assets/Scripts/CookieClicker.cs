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

    public int cookieClicked = 0;

    private float timer = 0;
    private float hours = 0;
    private float minutes = 0;
    private float seconds = 0;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            cookie = cookie.GetComponent<Button>();
            cookie.onClick.AddListener(CookieClicked);
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


    }

    void CookieClicked()
    {
        cookieClicked++;
        Debug.Log("Button Clicked");
    }
}
