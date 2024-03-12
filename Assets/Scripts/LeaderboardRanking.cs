using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardRanking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI time;

    public void Reset()
    {
        this.rank.text = "";
        this.username.text = "";
        this.time.text = "";
    }

    public void Set(EnterLeaderboard highScore)
    {
        this.rank.text = highScore.Rank.ToString() + ".";
        this.username.text = highScore.Username;
        this.time.text = TimeSpan.FromSeconds(highScore.Time).ToString(@"mm\:ss");

        if(highScore.IsUserScore)
        {
            Color green = new Color32(207, 198, 0, 255);
            this.rank.color = green;
            this.username.color = green;
            this.time.color = green;
        }
        else
        {
            Color white = new Color32(255, 255, 255, 255);
            this.rank.color = white;
            this.username.color = white;
            this.time.color = white;
        }
    }
}
